using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class PointClickMoment : MonoBehaviour {

    [SerializeField]
    private Transform target;

    public float rotSpeed = 15.0f;
    public float moveSpeed = 6.0f;
    public float jumpSpeed = 15.0f;
    public float gravity = -9.8f;
    public float terminalVelocity = -10.0f;
    public float minFall = -1.5f;

    public float pushForce = 3.0f;

    public float deceleration = 20.0f;
    public float targetBuffer = 1.5f;

    private float _vertSpeed;
    private float _curSpeed = 0f;
    private Vector3 _targetPos = Vector3.one;

    private Animator _animator;

    private CharacterController _charController;

    private ControllerColliderHit _contact; // Хранит данные о столкновении между функциями

    // Use this for initialization
    void Start()
    {
        _vertSpeed = minFall; // Минимальная скорость падения
        _charController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 movement = Vector3.zero; // Начинаем с вектора (0, 0, 0), непрерывно добаляя компоненты движения


        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject()) // Задаем целевую точку по щелчку мыши
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit mouseHit;
            if (Physics.Raycast(ray, out mouseHit))
            {
                GameObject hitObject = mouseHit.transform.gameObject;
                if(hitObject.layer == LayerMask.NameToLayer("Ground"))
                {
                    _targetPos = mouseHit.point; // Устанавливаем цель в точке попадания луча
                    _curSpeed = moveSpeed;
                }
            }
        }

        if (_targetPos != Vector3.one) // Перемещаем при заданной целевой точке
        {
            Vector3 adjustedPos = new Vector3(_targetPos.x, transform.position.y, _targetPos.z);
            Quaternion targetRot = Quaternion.LookRotation(adjustedPos - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotSpeed * Time.deltaTime); // Поворачиваем по напр к цели

            movement = _curSpeed * Vector3.forward;
            movement = transform.TransformDirection(movement);

            if (Vector3.Distance(_targetPos, transform.position) < targetBuffer)
            {
                _curSpeed -= deceleration * Time.deltaTime; // Снижаем скорость до нуля при приближении к цели

                if (_curSpeed <= 0)
                {
                    _targetPos = Vector3.one;
                }
            }
        }

        _animator.SetFloat("Speed", movement.sqrMagnitude);

        bool hitGround = false;
        RaycastHit hit;

        if (_vertSpeed < 0 && Physics.Raycast(transform.position, Vector3.down, out hit)) // Падает ли персонаж
        {
            float check = (_charController.height + _charController.radius) / 1.9f; // Расстояние с которым справнивает, немного ниже капсулы
            hitGround = hit.distance <= check;
        }

        if (hitGround) // Если соприкосается с поверхностью
        {
            //if (Input.GetButtonDown("Jump")) // Реакция на кнопку Jump
            //{
            //    _vertSpeed = jumpSpeed;
            //}
            //else
            //{
            //    _vertSpeed = -0.1f;
            //    _animator.SetBool("Jumping", false);
            //}
        }
        else // Если не стоит на поверхности применяем гравитацию до достижения максимальной скорости
        {
            _vertSpeed += gravity * 5 * Time.deltaTime;
            if (_vertSpeed < terminalVelocity)
            {
                _vertSpeed = terminalVelocity;
            }

            if (_contact != null) // Не следует вводить в дейстие это значение в самом начале уровня
            {
                _animator.SetBool("Jumping", true);
            }

            if (_charController.isGrounded) // Метод луча не обнаруживает повернхости, но капсула с ней соприкосается
            {
                if (Vector3.Dot(movement, _contact.normal) < 0) // Реакция меняется, если перс смотрит в сторону точки контакта
                {
                    movement = _contact.normal * moveSpeed;
                }
                else
                {
                    movement += _contact.normal * moveSpeed;
                }
            }
        }

        movement.y = _vertSpeed;
        movement *= Time.deltaTime;      // Независимость от кадра
        _charController.Move(movement);
    }


    void OnControllerColliderHit(ControllerColliderHit hit) // При распозновании столкновения, данные сохраняются в callback-е
    {
        _contact = hit;

        Rigidbody body = hit.collider.attachedRigidbody; // Проверка, есть ли у участвующего в столкновении объекта компонент Rigitbody,
        if (body != null && !body.isKinematic)            // обеспечивающий реакцию на приложенную силу
        {
            body.velocity = hit.moveDirection * pushForce; // Назначение физическому телу скорости
        }
    }


}
