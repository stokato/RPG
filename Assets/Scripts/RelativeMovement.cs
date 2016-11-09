using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class RelativeMovement : MonoBehaviour {

    [SerializeField]
    private Transform target;

    public float rotSpeed = 15.0f;
    public float moveSpeed = 6.0f;
    public float jumpSpeed = 15.0f;
    public float gravity = -9.8f;
    public float terminalVelocity = -10.0f;
    public float minFall = -1.5f;

    public float pushForce = 3.0f;

    private float _vertSpeed;

    private Animator _animator;

    private CharacterController _charController;

    private ControllerColliderHit _contact; // Хранит данные о столкновении между функциями

	// Use this for initialization
	void Start () {
        _vertSpeed = minFall; // Минимальная скорость падения
        _charController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
       
        Vector3 movement = Vector3.zero; // Начинаем с вектора (0, 0, 0), непрерывно добаляя компоненты движения

        float horInput = Input.GetAxis("Horizontal");
        float vertInput = Input.GetAxis("Vertical");

        if(horInput != 0 || vertInput != 0) // Движение обрабатывается только от клавиш со стрелками
        {
            
            movement.x = horInput * moveSpeed;
            movement.z = vertInput * moveSpeed;
            movement = Vector3.ClampMagnitude(movement, moveSpeed);

            Quaternion tmp = target.rotation; // Сохраняем начальную ориентацию

            target.eulerAngles = new Vector3(0, target.eulerAngles.y, 0);

            movement = target.TransformDirection(movement); // Из локальных координат в глобальные

            target.rotation = tmp;

            Quaternion direction = Quaternion.LookRotation(movement); // Вычесляем кватернион, смотрящий в этом направлении
            transform.rotation = Quaternion.Lerp(transform.rotation, direction, rotSpeed * Time.deltaTime); // Линейная интерполяция
        }

        _animator.SetFloat("Speed", movement.sqrMagnitude);

        bool hitGround = false;
        RaycastHit hit;

        if(_vertSpeed < 0 && Physics.Raycast(transform.position, Vector3.down, out hit)) // Падает ли персонаж
        {
            float check = (_charController.height + _charController.radius) / 1.9f; // Расстояние с которым справнивает, немного ниже капсулы
            hitGround = hit.distance <= check;
        }

        if (hitGround) // Если соприкосается с поверхностью
        {
            if (Input.GetButtonDown("Jump")) // Реакция на кнопку Jump
            {
                _vertSpeed = jumpSpeed;
            }
            else
            {
                _vertSpeed = -0.1f;
                _animator.SetBool("Jumping", false);
            }
        }
        else // Если не стоит на поверхности применяем гравитацию до достижения максимальной скорости
        {
            _vertSpeed += gravity * 5 * Time.deltaTime;
            if (_vertSpeed < terminalVelocity)
            {
                _vertSpeed = terminalVelocity;
            }

            if(_contact != null) // Не следует вводить в дейстие это значение в самом начале уровня
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
        if(body != null && !body.isKinematic)            // обеспечивающий реакцию на приложенную силу
        {
            body.velocity = hit.moveDirection * pushForce; // Назначение физическому телу скорости
        }
    }

    
}
