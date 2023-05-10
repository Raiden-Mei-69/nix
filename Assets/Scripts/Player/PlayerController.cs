using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    using Data;
    using Detector;
    using Loot;
    using Magic;
    using Player.PlayerStat;
    using Player.Skill;
    using Save;
    using System;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using UI;
    using Ultimate;
    using UnityEngine.AddressableAssets;
    using Weapon;
    using World;

    public class PlayerController : MonoBehaviour, ITarget
    {
        private UserData userData;
        public GameObject CharacterMeshHolder;
        public Vector3 scaleFactor;

        [Header("Data")]
        public PlayerData playerData;
        public int id;
        public bool LoadPosition = false;
        public CharacterStat characterStat;


        [Header("Component")]
        [SerializeField] internal CharacterController controller;
        [SerializeField] internal PlayerCollector collector;
        [SerializeField] internal PlayerTargetDetector targetDetector;
        [SerializeField] internal Animator _animator;
        [SerializeField] private PlayerInput playerInput;
        [SerializeField] internal PlayerUI playerUI;
        [SerializeField] internal Cheats.CheatsConsole cheatsConsole;
        public Save.Profile.ProfileSave profile;
        public Menu.PauseMenu pauseMenu;

        [Header("Target")]
        [SerializeField] private Transform _targetPoint;
        public Transform TargetPoint => _targetPoint;
        [Space(10)]

        #region Animation_ID
        private int _attackID;
        private int _walkingID;
        private int _runningID;
        private int _jumpID;
        private int _fallingID;
        private int _rollID;
        private int _idleWeaponID;
        private int _ultimateID;
        private int _swimmingID;
        private int _fallTriggerID;
        //internal int _comboCountID;
        //internal int _blendAttack;
        //private int _striffingID;
        #endregion

        public bool usingController = false;
        public bool inMenu = false;

        [Header("Cam")]
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private Cinemachine.CinemachineVirtualCamera _mainCameraVirtualCamera;
        [SerializeField] private Cinemachine.CinemachineVirtualCamera _cameraLock;
        [SerializeField] private Vector2 LookingInput = new(0, 0);
        //[SerializeField] private Vector2 aimSensitivity = new(1, 1);
        //[SerializeField] private Vector2 aimSensitivityController = new(1, 1);
        [SerializeField, Tooltip("X=>bottom / Y=>Top")] private Vector2 ClampY = new(-70, 70);
        [SerializeField] private float yaw = 0f;
        [SerializeField] private float pitch = 0f;
        [SerializeField] private float camOverride = 3f;
        [SerializeField] private float _targetRotation = 0f;
        [SerializeField] private float _rotSmoothWalking = 500f;
        private const float _treshold = 0.01f;
        public bool LockCameraPosition = false;

        [Header("Movement")]
        [SerializeField] private Vector2 MoveInput = new(0, 0);
        [SerializeField] private float moveSpeed = 1f;
        [SerializeField] private float SprintingSpeed = 5f;
        [SerializeField] private Vector3 _move;
        [SerializeField] private bool Sprinting = false;
        private float _speed;
        private float rotationSmooth = 0f;
        float _rotationVelocity;
        internal bool canMove = true;
        [SerializeField] private LayerMask waterMask;
        [SerializeField] private float offsetSwimmingController = .87f;
        [SerializeField] private float offsetControllerLand = 1f;
        private bool _inWater = false;
        private Coroutine routineSwim = null;


        [Header("Dash")]
        [SerializeField] private bool _canDash = true;
        [SerializeField] private float _delayDash = 1.5f;
        [SerializeField] private float _dashLength = 1f;
        [SerializeField] internal float _dashTime = 1f;
        [SerializeField] private float _dashSpeed = 30f;
        [SerializeField] internal bool _invincible = false;
        [SerializeField] internal bool _inDash = false;
        [SerializeField] private AnimationCurve dodgeCurve;
        [SerializeField] private float dashVelocity = 1f;
        [SerializeField] private float _runDelay = .5f;

        [Space(10), Header("Jump")]
        [SerializeField] private bool JumpAction = false;
        [SerializeField] private float jumpForce = 5f;
        [SerializeField] private bool Grounded = false;
        [SerializeField] private bool _touchingSomething = false;
        [SerializeField] private LayerMask _thingsLayer;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] internal float Gravity = 0f;
        [SerializeField] private float GroundedOffset = 1f;
        [SerializeField] private float GroundedRadius = .2f;
        [SerializeField] private float _verticalVelocity = 0f;
        [SerializeField] private Vector3 GroundOffset;
        [SerializeField] private float _fallTimeoutDelta;
        [SerializeField] private float _jumpTimeoutDelta;
        [SerializeField] private float _terminalvelocity = 53f;
        private const float JumpTimeout = .5f;
        private const float FallTimeout = .15f;
        private Coroutine routineFalling = null;

        [Header("Attack")]
        public bool inUltimate = false;
        [SerializeField] private bool _weaponDraw = false;
        public SheathingWeaponHolder sheathedWeaponHolder;
        [SerializeField] internal WeaponHolder WeaponHolder;
        [SerializeField] internal PlayerWeapon playerWeapon;
        [SerializeField] internal LayerMask HitLayer;
        [SerializeField] internal bool _isAttacking = false;
        internal int comboCount = 0;
        internal bool _canAttack = true;
        public int MaxComboCount = 10;
        public int Damage = 20;
        [SerializeField] private bool lockOn = false;
        public List<Transform> PotentialTargets = new();
        public Transform currentTarget;
        [SerializeField] private float rotationSmoothLock;
        public float delayLastCombo = .5f;

        private Coroutine routineSprinting;
        private bool allowCamMove = false;
        private Coroutine routineAttack;
        private bool canChangeTarget = true;

        [Space(5),Header("Magic Atttack")]
        public PlayerSkill playerSkill;

        [Space(5),Header("Ultimate")]
        public PlayerUltimateWeapon ultimateWeapon;
        private bool ultimateActive = false;
        public Transform ultimateHolder;
        public UltimateMagic ultimateMagic;

        //Awake is called before the Start
        private void Awake()
        {
            scaleFactor = CharacterMeshHolder.transform.localScale;
            userData = new();
            routineSprinting = null;
            GetAnimID();
            WeaponHolder = WeaponHolder != null ? WeaponHolder : GetComponentInChildren<WeaponHolder>();
            EquipWeapon();
            _isAttacking = false;
            comboCount = -1;
            playerUI.gameObject.SetActive(true);
            cheatsConsole.ForceClose();
            Load();
        }

        [ContextMenu("Load")]
        private void Load()
        {
            if (playerData.Load(Save.SaveManager.LoadSave(), this))
            {
                if (LoadPosition)
                {
                    transform.position = playerData.Position;
                    playerData.id = userData.uid;
                }
                if (playerData.inventory.UltimatePowerSlot.PowerPath != string.Empty)
                {
                    var go = Addressables.InstantiateAsync(playerData.inventory.UltimatePowerSlot.PowerPath, parent: ultimateHolder);
                    ultimateMagic = go.WaitForCompletion().GetComponent<UltimateMagic>();
                    //AddressablesPath.ClearOps(go);
                }
            }
        }

        public void EquipWeapon(PlayerWeapon weap = null)
        {
            if (weap != null)
            {
                WeaponHolder.PlayerWeapon = weap;
                WeaponHolder.OnWeaponChange(weap);
            }
            playerWeapon = WeaponHolder.PlayerWeapon;
        }

        private void GetAnimID()
        {
            _attackID = Animator.StringToHash("Attack");
            _walkingID = Animator.StringToHash("Walking");
            _fallingID = Animator.StringToHash("Falling");
            _jumpID = Animator.StringToHash("Jump");
            _rollID = Animator.StringToHash("Roll");
            _runningID = Animator.StringToHash("Running");
            _idleWeaponID = Animator.StringToHash("IdleWeapon");
            _ultimateID = Animator.StringToHash("Ultimate");
            _swimmingID = Animator.StringToHash("Swimming");
            _fallTriggerID = Animator.StringToHash("Fall");
            //_comboCountID = Animator.StringToHash("Combo");
            //_blendAttack = Animator.StringToHash("AttackCombo");
            //_striffingID = Animator.StringToHash("Striffing");
        }

        // Start is called before the first frame update
        IEnumerator Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            yield return new WaitForSeconds(.5f);
            allowCamMove = true;
            routineAttack = null;
            //SendSave();
        }


        public void Quitting()
        {
            playerData.Position = transform.position;
            SaveManager.SaveGame(playerData.Stringify());
            SaveManager.SaveFlut(playerData.Stringify());
#if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode();
#else
            Application.Quit();
#endif
        }

        public void OnPauseMenuClose()
        {
            IEnumerator WaitJustALittle()
            {
                yield return new WaitForEndOfFrame();
                yield return new WaitForFixedUpdate();
                yield return null;
                canMove = true;
            }

            StartCoroutine(WaitJustALittle());
        }

        public void SheathWeapon()
        {
            Debug.Log("Sheathed");
            sheathedWeaponHolder.OnSeathing(playerWeapon);
            _weaponDraw = false;
        }

        public void DrawWeapon()
        {
            Debug.Log("Draw");
            WeaponHolder.DrawWeapon(playerWeapon);
            _animator.SetBool(_idleWeaponID, _weaponDraw = true);
        }

        // Update is called once per frame
        void Update()
        {
            if (inMenu)
                return;

            if (Keyboard.current.numpad0Key.wasPressedThisFrame)
            {
                ultimateActive = !ultimateActive;
                if (ultimateActive)
                {
                    ultimateWeapon.OnStart();
                }
                else
                {
                    ultimateWeapon.OnStop();
                }
            }

            if (Keyboard.current.numpad8Key.wasPressedThisFrame)
            {
                SheathWeapon();
            }
            else if (Keyboard.current.numpad9Key.wasPressedThisFrame)
            {
                DrawWeapon();
            }

            if (!_isAttacking && canMove || !_inDash && canMove)
            {
                Movement();
                JumpAndGravity();
            }
            else
            {
                //Debug.Log("Cant move");
            }
            if (Keyboard.current.f1Key.wasPressedThisFrame)
                Quitting();
            else if (Keyboard.current.leftBracketKey.wasPressedThisFrame)
            {
                //Debug.Log("Prev");
                Teleport(TeleportManager.Instance.GetTeleporter());
            }
            else if (Keyboard.current.rightBracketKey.wasPressedThisFrame)
            {
                //Debug.Log("Next");
                Teleport(TeleportManager.Instance.GetTeleporter(false));
            }
            else if (Keyboard.current.oKey.wasPressedThisFrame)
            {
                TakeDamage(1);
            }
            else if (Keyboard.current.pKey.wasPressedThisFrame)
            {
                TakeDamage(-1);
            }
            else if (Keyboard.current.lKey.wasPressedThisFrame)
            {
                RemoveMagic(1);
            }
            else if (Keyboard.current.semicolonKey.wasPressedThisFrame)
            {
                RemoveMagic(-1);
            }
            else if (Keyboard.current.f10Key.wasPressedThisFrame)
            {
                RemoveMagic(-playerData.MaxMagic);
            }
            else if (Keyboard.current.f9Key.wasPressedThisFrame)
            {
                TakeDamage(-playerData.ModifiedMaxHealth);
            }


            if (inMenu && playerUI._characterTabHolder.activeSelf)
            {
                if (Keyboard.current != null)
                {
                    if (Keyboard.current.escapeKey.wasPressedThisFrame)
                    {
                        inMenu = false;
                        playerUI.ShowCharacterDetail(inMenu);
                    }
                }
            }
            if (Keyboard.current != null)
            {
                if (Keyboard.current.bKey.wasPressedThisFrame)
                {
                    inMenu = !inMenu;
                    playerUI.ToggleInventory(inMenu);
                }
            }
        }

        private void LateUpdate()
        {
            if (inMenu)
                return;
            if (allowCamMove)
                CameraRotation();
        }

        #region Input_Event
        public void LookInputEvent(InputAction.CallbackContext ctx)
        {
            if (!lockOn)
                LookingInput = ctx.ReadValue<Vector2>();
        }
        public void MoveInputEvent(InputAction.CallbackContext ctx)
        {
            MoveInput = ctx.ReadValue<Vector2>();
        }
        public void JumpInputEvent(InputAction.CallbackContext ctx)
        {
            if (ctx.performed && !inMenu)
            {
                //Debug.Log("Jumped");
                JumpAction = ctx.ReadValueAsButton();
                //JumpAction = ctx.performed;
                //_animator.SetTrigger(_jumpID);
            }
        }
        public void AttackInputEvent(InputAction.CallbackContext ctx)
        {
            if (ctx.performed && !inMenu)
            {
                DelayAttackCombo();
                //_isAttacking = true;
                //ComboSolver();
                //DelayAttackCombo(.75f);
                //_animator.SetInteger(_comboCountID, comboCount);
                //_animator.SetFloat(_blendAttack, comboCount);
                //_animator.SetTrigger(_attackID);
                //_isAttacking = false;
            }
            //gunManager.Shoot(ctx);
        }
        public void SprintInputAction(InputAction.CallbackContext ctx)
        {
            if (inMenu)
                return;
            if (ctx.performed)
            {
                StartCoroutine(HandleSprintOrDash(ctx));
            }
        }
        public void InteractInputAction(InputAction.CallbackContext ctx)
        {
            if (inMenu)
                return;
            if (ctx.performed)
            {
                Interact(ctx);
            }
        }
        public void LockInputAction(InputAction.CallbackContext ctx)
        {
            if (ctx.performed && !inMenu && false)
            {
                PotentialTargets.RemoveAll((item) => item == null || !item.GetComponentInParent<Enemy.EnemyBase>().targetableLocal);
                if (PotentialTargets.Count > 0)
                {
                    lockOn = !lockOn;
                    if (lockOn)
                    {
                        HandleTarget();
                    }
                    else
                    {
                        HandleTargetLost();
                    }
                    ChangeCamera();
                }
            }
        }

        private void ChangeCamera()
        {
            _cameraLock.gameObject.SetActive(lockOn);
            _mainCameraVirtualCamera.gameObject.SetActive(!lockOn);
        }

        public void LockTargetChangeInputAction(InputAction.CallbackContext ctx)
        {

            if (ctx.performed && canChangeTarget && !inMenu)
            {
                Vector2 input = ctx.ReadValue<Vector2>();
                if (usingController)
                {
                    if (input.x >= .80f)
                    {
                        HandleCycleTarget(true);
                    }
                    else if (input.x <= -.80f)
                    {
                        HandleCycleTarget(false);
                    }
                }
                else
                {
                    if (input.y >= 100f)
                    {
                        HandleCycleTarget(true);
                    }
                    else if (input.y <= -100f)
                    {
                        HandleCycleTarget(false);
                    }
                }
            }
        }
        public void DeviceChange()
        {
            usingController = playerInput.currentControlScheme == "Gamepad";
            //Debug.Log("Device chnage");
        }
        public void MagicAttackInput(InputAction.CallbackContext ctx)
        {
            if (ctx.performed && !inMenu)
            {
                StartCoroutine(MagicAttack());
            }
        }
        public void UltimateInput(InputAction.CallbackContext ctx)
        {
            if (!inMenu)
            {
                //Debug.Log("<color=green>Ult</color>");
                StartCoroutine(UltimateTrigger(ctx));
            }
        }

        public void CheatsConsoleEvents(InputAction.CallbackContext ctx)
        {
            if (pauseMenu.gameObject.activeSelf || playerUI.TabActive)
                return;
            if (ctx.performed)
            {
                cheatsConsole.ChangeState();
            }
        }
        public void InventoryInputEvents(InputAction.CallbackContext ctx)
        {
            if (pauseMenu.gameObject.activeSelf)
                return;
            if (ctx.performed)
            {
                inMenu = !inMenu;
                playerUI.ShowCharacterDetail(inMenu);
                _mainCamera.gameObject.SetActive(!inMenu);
                GameManager.Instance.PauseGame(inMenu);
                Cursor.visible = inMenu;
                Cursor.lockState = inMenu ? CursorLockMode.None : CursorLockMode.Locked;
            }
        }
        public void PauseGameInputEvents(InputAction.CallbackContext ctx)
        {
            if (playerUI.TabActive)
                return;
            if (ctx.performed)
            {
                inMenu = !inMenu;
                Cursor.visible = inMenu;
                Cursor.lockState = inMenu ? CursorLockMode.None : CursorLockMode.Locked;
                pauseMenu.ChangeState(inMenu);
            }
        }
        public void ShowCursorInputEvents(InputAction.CallbackContext ctx)
        {

        }
        #endregion

        public void ShowInteract(bool state, string name = "")
        {
            playerUI.ShowCollectable(state, name);
        }

        private void Movement()
        {
            if (_isAttacking)
            {
                _animator.SetBool(_walkingID, false);
                return;
            }
            //if (currentTarget == null)
            //    _animator.SetBool(_striffingID, false);
            float targetSpeed = Sprinting ? SprintingSpeed : moveSpeed;
            //_move = transform.right * MoveInput.x + transform.forward * MoveInput.y;
            //controller.Move(_move.normalized * (targetSpeed * Time.deltaTime) + new Vector3(0f, _verticalVelocity, 0f) * Time.deltaTime);

            if (MoveInput == Vector2.zero)
                targetSpeed = 0f;
            float currentHorizontalspeed = new Vector3(controller.velocity.x, 0f, controller.velocity.z).magnitude;
            float speedOffset = .1f;
            float inputMagnitude = MoveInput.magnitude > .1f ? MoveInput.magnitude : 1f;
            if (currentHorizontalspeed < targetSpeed - speedOffset || currentHorizontalspeed > targetSpeed + speedOffset)
            {
                _speed = Mathf.Lerp(currentHorizontalspeed, targetSpeed * inputMagnitude, Time.deltaTime * 10);
                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else
            {
                _speed = targetSpeed;
            }
            if (inUltimate&&targetSpeed!=0)
            {
                _speed = ultimateWeapon.moveSpeedDuringUltimate;
                Debug.Log(_speed);
            }
            Vector3 inputDirection = new Vector3(MoveInput.x, 0f, MoveInput.y).normalized;
            if (MoveInput != Vector2.zero)
            {
                _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + _mainCamera.transform.eulerAngles.y;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, rotationSmooth);
                transform.rotation = Quaternion.Euler(0f, rotation, 0f);
            }
            Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

            controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
            if (!lockOn)
                _animator.SetBool(_walkingID, MoveInput != Vector2.zero);
            else
                LockAtTarget();
            //If we move then we sheath the weapon
            if (MoveInput != Vector2.zero)
            {
                _animator.SetBool(_idleWeaponID, _weaponDraw);
                SheathWeapon();
            }
        }

        private void JumpAndGravity()
        {
            if (_isAttacking || inUltimate)
                return;
            GroundCheck();
            if (Grounded)
            {
                //print("if");
                _fallTimeoutDelta = FallTimeout;
                if (_verticalVelocity < 0f)
                    _verticalVelocity = -2f;
                if (JumpAction && _jumpTimeoutDelta <= 0f)
                {
                    _animator.SetTrigger(_jumpID);
                    _verticalVelocity = Mathf.Sqrt(jumpForce * -2f * Gravity);
                }
                if (_jumpTimeoutDelta >= 0f)
                    _jumpTimeoutDelta -= Time.deltaTime;
            }
            else
            {
                if (_touchingSomething)
                {
                    _jumpTimeoutDelta = JumpTimeout;
                    JumpAction = false;
                    //_verticalVelocity = 0f;
                    //Debug.Log("touchy");
                }
                else
                {
                    //print("else");
                    _jumpTimeoutDelta = JumpTimeout;
                    if (_fallTimeoutDelta >= 0f)
                        _fallTimeoutDelta -= Time.deltaTime * Gravity;
                    JumpAction = false;
                    SheathWeapon();
                }
            }

            if (_verticalVelocity < _terminalvelocity)
            {
                //Debug.Log("Falling");
                _verticalVelocity += Gravity * Time.deltaTime;
            }
            _animator.SetBool(_idleWeaponID, _weaponDraw);
        }

        private void GroundCheck()
        {
            Vector3 spherePosition = new(transform.position.x - GroundOffset.x, transform.position.y - GroundOffset.y, transform.position.z - GroundOffset.z);
            Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, groundLayer);
            _inWater = Physics.CheckSphere(spherePosition, GroundedRadius, waterMask);
            if (_inWater&&routineSwim==null)
            {
                routineSwim=StartCoroutine(InSwimming());
            }
            if (!Grounded)
            {
                _touchingSomething = Physics.CheckSphere(spherePosition, _thingsLayer);
            }
            else
            {
                _touchingSomething = false;
            }
            if (!_inDash && !_inWater && routineFalling == null&&!Grounded)
                routineFalling = StartCoroutine(InAir());
                //_animator.SetBool(_fallingID, !Grounded);
        }

        private void CameraRotation()
        {
            Vector2 targetSensitivity = usingController ? profile.camSensitivityController : profile.camSensitivityMouse;
            if (LookingInput.sqrMagnitude >= _treshold && !LockCameraPosition)
            {
                yaw += (LookingInput.x * Time.deltaTime) * targetSensitivity.x;
                pitch += ((LookingInput.y * Time.deltaTime) * targetSensitivity.y) * -1f;
            }
            yaw = ClampAngle(yaw, float.MinValue, float.MaxValue);
            pitch = ClampAngle(pitch, ClampY[0], ClampY[1]);

            //camHolder.transform.rotation = Quaternion.Euler(pitch + camOverride, yaw, 0f);
            //transform.rotation = Quaternion.Euler(0, yaw, 0);
            _mainCameraVirtualCamera.LookAt.transform.rotation = Quaternion.Euler(pitch + camOverride, yaw, 0.0f);
        }

        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

        IEnumerator HandleSprintOrDash(InputAction.CallbackContext ctx)
        {
            float delay = _runDelay / 10f;
            bool exec = true;
            if (ctx.action.WasReleasedThisFrame())
            {
                StartCoroutine(DashCoroutine());
                exec = false;
                yield break;
            }

            do
            {
                if (ctx.action.WasReleasedThisFrame())
                {
                    StartCoroutine(DashCoroutine());
                    yield break;
                }
                for (int i = 0; i < 10; i++)
                {
                    //Debug.Log("Press");
                    yield return new WaitForSeconds(delay);
                    if (!ctx.action.IsPressed())
                        break;
                }
                if (ctx.action.IsPressed())
                {
                    Sprinting = true;
                    _animator.SetBool(_runningID, Sprinting);
                }
                else
                    break;
            } while (ctx.action.inProgress && exec && ctx.action.IsPressed() && !ctx.action.WasPerformedThisFrame());
            if (!Sprinting)
                StartCoroutine(DashCoroutine());
            Sprinting = false;
            _animator.SetBool(_runningID, Sprinting);
        }

        private IEnumerator HandleSprintingInput(InputAction.CallbackContext ctx)
        {
            //print("Call");
            if (ctx.performed && MoveInput != Vector2.zero)
            {
                Sprinting = true;
                yield return new WaitUntil(() => MoveInput == Vector2.zero);
                routineSprinting = null;
                Sprinting = false;
            }
            else
            {
                yield return null;
            }
        }

        private IEnumerator DashCoroutine()
        {
            if (!_canDash || !Grounded || _isAttacking || _inDash || inUltimate)
                yield break;
            float timer = 0f;
            _canDash = false;
            _animator.SetTrigger(_rollID);

            //Rotation
            _targetRotation = Mathf.Atan2(MoveInput.x, MoveInput.y) * Mathf.Rad2Deg + _mainCamera.transform.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, rotationSmooth);
            transform.rotation = Quaternion.Euler(0f, rotation, 0f);

            while (timer < _dashTime)
            {
                if (lockOn)
                {
                    _targetRotation = Mathf.Atan2(MoveInput.x, MoveInput.y) * Mathf.Rad2Deg + _mainCamera.transform.eulerAngles.y;
                    float rot = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, rotationSmooth);
                    transform.rotation = Quaternion.Euler(0f, rot, 0f);
                }

                float speed = dodgeCurve.Evaluate(timer);
                Vector3 dir = ((transform.forward * speed) * _dashSpeed) + (Vector3.up * dashVelocity);
                controller.Move(dir * Time.deltaTime);
                timer += Time.deltaTime;
                yield return null;
            }
            _canDash = true;
        }

        public void ComboSolver()
        {
            if (!_canAttack)
                return;
            if (comboCount == 0)
                _animator.SetTrigger(_attackID);
            if (comboCount <= MaxComboCount)
            {
                comboCount++;
            }
            else
            {
                comboCount = 0;
            }
        }

        IEnumerator ResetAttackCombo()
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitForSeconds(_animator.GetAnimatorTransitionInfo(0).duration);
            yield return new WaitForEndOfFrame();
            yield return new WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f);
            comboCount = -1;
            //_animator.SetInteger(_comboCountID, comboCount);
            _isAttacking = false;
        }

        public void DelayAttackCombo()
        {
            if (!Grounded || !_canAttack)
                return;

            //if(comboCount == MaxComboCount)
            //    return;
            float time = _animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            float delayTime = time * 0.75f;
            float restAnim = time - delayTime;
            _isAttacking = true;
            DrawWeapon();

            if (comboCount == -1 || (time >= 0.1f && time <= 0.8f))
            {

                //Debug.Log("<color=yellow>here</color>");
                if (routineAttack != null)
                    StopCoroutine(routineAttack);
                comboCount++;
                //if (comboCount == 0)
                //    _animator.SetTrigger(_attackID);
                _animator.SetTrigger(_attackID);
                //_animator.SetInteger(_comboCountID, comboCount);
                PotentialTargets.RemoveAll((item) => item == null);
                if (PotentialTargets.Count > 0)
                {
                    Transform target = PotentialTargets.First();
                    Vector3 dir = target.position - transform.position;
                    dir.y = 0;
                    Quaternion rot = Quaternion.LookRotation(dir);
                    transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * rotationSmoothLock);
                }
                routineAttack = StartCoroutine(ResetAttackCombo());
            }
        }

        public void AttackAnimFinished()
        {
            Debug.Log("Trigger");
            playerWeapon.hits.Clear();
        }

        public void AttackTrigger()
        {
            if (!_canAttack)
                return;
        }

        public void LockAtTarget()
        {
            if (currentTarget == null)
            {
                lockOn = false;
                ChangeCamera();
                return;
            }
            Vector3 dir = currentTarget.position - transform.position;
            dir.y = 0;
            Quaternion rot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * rotationSmoothLock);
            _animator.SetBool(_walkingID, false);
            //if (MoveInput != Vector2.zero)
            //    _animator.SetBool(_striffingID, true);
            //else
            //    _animator.SetBool(_striffingID, false);
        }

        public void GiveXP(int amount)
        {
            playerData.AddXP(amount);
            PotentialTargets.RemoveAll((item) => item == null);
            playerUI.UpdateBar();
        }

        public void HandleTarget()
        {
            currentTarget = PotentialTargets.First();
            _cameraLock.LookAt = currentTarget;
            Enemy.EnemyBase enemy = currentTarget.GetComponentInParent<Enemy.EnemyBase>();
            enemy.player = this;
            enemy.isTarget = true;
        }

        private IEnumerator DelayChangeTarget()
        {
            canChangeTarget = false;
            yield return new WaitForSeconds(.75f);
            canChangeTarget = true;
        }

        public void HandleCycleTarget(bool up)
        {
            if (PotentialTargets.Count == 0 || PotentialTargets.Count == 1)
                return;
            //Debug.Log("Changing");
            int index = PotentialTargets.IndexOf(currentTarget);
            if (up)
            {
                if (index + 1 < PotentialTargets.Count)
                {
                    currentTarget = PotentialTargets[index + 1];
                }
                else
                {
                    currentTarget = PotentialTargets.First();
                }
            }
            else
            {
                if (index - 1 > 0)
                {
                    currentTarget = PotentialTargets[index - 1];
                }
                else
                {
                    currentTarget = PotentialTargets.Last();
                }
            }
            _cameraLock.LookAt = currentTarget;
            currentTarget.GetComponentInParent<Enemy.EnemyBase>().isTarget = true;
            StartCoroutine(DelayChangeTarget());
        }

        public void HandleTargetLost()
        {
            Enemy.EnemyBase enemy = currentTarget.GetComponentInParent<Enemy.EnemyBase>();
            enemy.isTarget = false;
            enemy.player = null;
        }

        public void Teleport(Vector3 tpPoint)
        {
            controller.enabled = false;
            transform.position = tpPoint;
            controller.enabled = true;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(new(transform.position, transform.forward * 5f));
            Color transparentGreen = new(0f, 1f, 0, .35f);
            Color transparentRed = new(1, 0, 0, .35f);
            Gizmos.color = Grounded ? transparentGreen : transparentRed;
            Gizmos.DrawSphere(new(transform.position.x - GroundOffset.x, transform.position.y - GroundOffset.y, transform.position.z - GroundOffset.z), GroundedRadius);
        }

        public void TakeDamage(int value)
        {
            playerData.Health -= value;
            playerUI.UpdateBar();
        }

        public void RemoveMagic(int value)
        {
            playerData.Magic -= value;
            playerUI.UpdateBar();
        }

        private void Interact(InputAction.CallbackContext ctx)
        {
            collector.Clean();
            if (collector.chests.Count > 0)
            {
                var chest = collector.chests.First();
                chest.GetLoots();
                collector.Clean();
                collector.RemoveLoot(chest: chest);
            }
            else if (collector.fishes.Count > 0)
            {
                Animal.Aquatic.Fish fish = collector.fishes.First();
                fish.Die();
                collector.RemoveLoot(fish: fish);
            }
            else if (collector.lootItems.Count > 0)
            {
                var item = collector.lootItems.First();
                playerData.inventory.GatherItem(collector.lootItems.First().GetItems());
                NotificationSystem.Instance.CreateNewNotif(collector.lootItems.First().GetItems().Key.Icon, $"{collector.lootItems.First().GetItems().Key.Name} X{collector.lootItems.First().GetItems().Value}");
                Destroy(collector.lootItems.First().gameObject);
                collector.lootItems.RemoveAt(collector.lootItems.IndexOf(collector.lootItems.First()));
                collector.RemoveLoot(item: item);
            }
            else if (collector.lootWeapons.Count > 0)
            {
                var weap = collector.lootWeapons.First();
                var set = weap.GetWeapon();
                playerData.inventory.CollectWeaponLoot(set);
                NotificationSystem.Instance.CreateNewNotif(set.icon, $"{set.WeaponName}");
                Destroy(weap.gameObject);
                collector.RemoveLoot(weapon: weap);
            }
            collector.CollectableChange();
        }

        private IEnumerator MagicAttack()
        {
            playerSkill.UseSkill();
            yield return null;
        }

        private IEnumerator UltimateTrigger(InputAction.CallbackContext ctx)
        {
            if (inUltimate)
                yield break;

            SheathWeapon();
            _canAttack = false;
            _canDash = false;
            //canMove = false;
            _animator.SetTrigger(_ultimateID);
            ultimateWeapon.OnStart();
            StartCoroutine(FaceEnemyDuringUlt());
            yield return null;
        }

        private IEnumerator FaceEnemyDuringUlt()
        {
            do
            {
                if (PotentialTargets.Count > 0)
                {
                    Transform target = PotentialTargets.First();
                    Vector3 dir = target.position - transform.position;
                    dir.y = 0;
                    Quaternion rot = Quaternion.LookRotation(dir);
                    transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * rotationSmoothLock);
                }
                yield return null;
            } while (inUltimate);
        }

        public void OnUltimateAnimEnded()
        {
            ultimateWeapon.OnStop();
            canMove = true;
            _isAttacking = !true;
            _canAttack = !false;
            _canDash = !false;
            DrawWeapon();
        }

        private IEnumerator InSwimming()
        {
            controller.center = new(controller.center.x, offsetSwimmingController, controller.center.z);
            Debug.Log("Swimming");
            _animator.SetBool(_swimmingID, _inWater);
            yield return new WaitUntil(()=>!_inWater);
            controller.center = new(controller.center.x, offsetControllerLand, controller.center.z);
            _animator.SetBool(_swimmingID, _inWater);
            routineSwim = null;
        }

        private IEnumerator InAir()
        {
            _animator.SetTrigger(_fallTriggerID);
            _animator.SetBool(_fallingID, true);
            yield return new WaitUntil(() => Grounded&&!_inWater);
            _animator.SetBool(_fallingID, false);
            routineFalling = null;
        }

        public async void OnLastAttackCombo()
        {
            //Debug.Log("Wait!!!");
            _canAttack = false;
            await Task.Delay(Mathf.RoundToInt(delayLastCombo * 1000));
            _canAttack = true;
            //Debug.Log("Completed!!!");
        }

        public void VFXWeapon()
        {
            playerWeapon.ActivateVFX();
        }
    }

    [Serializable]
    public class UserData
    {
        public string username = "DRAGONLORDS";
        public long uid = 209257679820947456;
        public bool connected = false;
        public DateTime connectedAt = DateTime.UtcNow;
        public DateTime lastConnectedAt = DateTime.UtcNow;

        public void Open()
        {
            connected = true;
            connectedAt = DateTime.UtcNow;
            lastConnectedAt = connectedAt;
        }

        public void Update()
        {
            connected = true;
            lastConnectedAt = DateTime.UtcNow;
        }

        public void Close()
        {
            connected = false;
            lastConnectedAt = DateTime.UtcNow;
        }
    }
}