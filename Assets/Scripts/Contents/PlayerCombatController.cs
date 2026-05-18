using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombatController : MonoBehaviour
{
    public PlayerAttack warriorAttack;
    public PlayerBowAttack archerAttack;

    public enum PlayerClass
    {
        Warrior,
        Archer
    }

    public PlayerClass currentClass = PlayerClass.Warrior;

    private PlayerInput playerInput;
    private InputAction attackAction;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        if (playerInput != null)
        {
            // Input Actions Asset에서 "Attack" 액션 참조
            attackAction = playerInput.actions["Attack"];
        }
    }

    private void Start()
    {
        SetClass(currentClass);
    }

    private void Update()
    {
        // 테스트용 클래스 전환
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            SetClass(PlayerClass.Warrior);
        }

        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            SetClass(PlayerClass.Archer);
        }
    }

    public void SetClass(PlayerClass newClass)
    {
        currentClass = newClass;

        // 기존 Attack 이벤트 전부 해제
        UnbindAttackEvents();

        // 컴포넌트 활성/비활성
        if (warriorAttack != null)
        {
            warriorAttack.enabled = (newClass == PlayerClass.Warrior);
        }

        if (archerAttack != null)
        {
            archerAttack.enabled = (newClass == PlayerClass.Archer);
        }

        // 새 클래스 기준 Attack 이벤트 재바인딩
        BindAttackEvent();

        Debug.Log("Current Class: " + newClass);
    }

    private void BindAttackEvent()
    {
        if (attackAction == null) return;

        switch (currentClass)
        {
            case PlayerClass.Warrior:
                if (warriorAttack != null)
                {
                    attackAction.performed += warriorAttack.OnAttack;
                }
                break;

            case PlayerClass.Archer:
                if (archerAttack != null)
                {
                    attackAction.performed += archerAttack.OnAttack;
                }
                break;
        }
    }

    private void UnbindAttackEvents()
    {
        if (attackAction == null) return;

        if (warriorAttack != null)
        {
            attackAction.performed -= warriorAttack.OnAttack;
        }

        if (archerAttack != null)
        {
            attackAction.performed -= archerAttack.OnAttack;
        }
    }

    private void OnDestroy()
    {
        UnbindAttackEvents();
    }
}
