using UnityEngine;
using UnityEngine.InputSystem;

namespace EchoesOfSyllis.Player
{
    /// <summary>
    /// Core controller responsible for processing player inputs, managing 2D physics-based 
    /// horizontal movement, handling jump mechanics, and executing ground-proximity detection.
    /// Follows professional architecture by dividing input reception from physical execution.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CapsuleCollider2D))]
    public class PlayerController : MonoBehaviour
    {
        // ==========================================
        // CONFIGURATION FIELDS (Inspector Adjustable)
        // ==========================================

        [Header("Movement Settings")]
        [Tooltip("The maximum horizontal velocity of the player in units per second.")]
        [SerializeField] private float moveSpeed = 8f;

        [Tooltip("The initial upward velocity applied to the physics body during a jump execution.")]
        [SerializeField] private float jumpForce = 12f;

        [Header("Ground Detection")]
        [Tooltip("The spatial origin point beneath the character used to anchor the ground check boundary.")]
        [SerializeField] private Transform groundCheckPoint;

        [Tooltip("The dimensions (width, height) of the physics overlap box used to sense the floor.")]
        [SerializeField] private Vector2 groundCheckSize = new Vector2(0.5f, 0.1f);

        [Tooltip("The specific physics layer tracking geometry categorized as walkable platforms.")]
        [SerializeField] private LayerMask groundLayer;

        // ==========================================
        // PRIVATE STATE VARIABLES
        // ==========================================

        private Rigidbody2D rb;
        private Vector2 moveInput;
        private bool isGrounded;

        // ==========================================
        // ENGINE LIFECYCLE METHODS
        // ==========================================

        /// <summary>
        /// Awake is invoked immediately upon scene initialization. 
        /// Ideal for establishing hard cached references to components on the same GameObject.
        /// </summary>
        private void Awake()
        {
            // Cache reference to the Rigidbody2D to eliminate internal runtime performance costs
            rb = GetComponent<Rigidbody2D>();
        }

        /// <summary>
        /// Update runs once per frame. This is utilized for non-physics operations, 
        /// frame-dependent logic updates, and constant sensory checks.
        /// </summary>
        private void Update()
        {
            // Query the physics engine every frame to ensure current landing states are accurate
            CheckGroundStatus();
        }

        /// <summary>
        /// FixedUpdate runs on a reliable, locked interval independent of rendering framerates.
        /// All modifications to rigidbodies and structural physics calculations MUST occur here.
        /// </summary>
        private void FixedUpdate()
        {
            // Execute the horizontal translation while preserving whatever vertical velocity 
            // gravity or jumping has currently given the Rigidbody2D.
            rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);
        }

        // ==========================================
        // INPUT SYSTEM CALLBACK EVENTS
        // ==========================================

        /// <summary>
        /// Event listener mapped directly to the Input System's Move action.
        /// Captures directional vector data from WASD, Arrow Keys, or Analog Sticks.
        /// </summary>
        /// <param name="context">The contextual framework data delivered by the Input System.</param>
        public void OnMove(InputAction.CallbackContext context)
        {
            // Extract the standard 2D vector coordinates from the incoming input stream
            moveInput = context.ReadValue<Vector2>();
        }

        /// <summary>
        /// Event listener mapped directly to the Input System's Jump action.
        /// Validates pressing states alongside ground states to execute a true physics leap.
        /// </summary>
        /// <param name="context">The contextual framework data delivered by the Input System.</param>
        public void OnJump(InputAction.CallbackContext context)
        {
            // .started checks for the initial down-press of the spacebar frame to avoid button-hold cheating.
            // isGrounded ensures the player cannot infinitely trigger jumps while airborne.
            if (context.started && isGrounded)
            {
                // Manipulate physics parameters by introducing an instantaneous vertical force surge
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            }
        }

        // ==========================================
        // CORE MECHANICS AND UTILITIES
        // ==========================================

        /// <summary>
        /// Projects a non-allocating bounding box into the 2D physics matrix to verify 
        /// structural alignment between the player's feet and the ground layer assets.
        /// </summary>
        private void CheckGroundStatus()
        {
            if (groundCheckPoint != null)
            {
                // Returns true if the defined box intersects any collider tagged with our chosen Ground LayerMask
                isGrounded = Physics2D.OverlapBox(
                    groundCheckPoint.position,
                    groundCheckSize,
                    0f,
                    groundLayer
                );
            }
            else
            {
                // Defensive programming safeguard: defaults to unsafe state if a tracking object is unassigned
                isGrounded = false;
            }
        }

        /// <summary>
        /// Built-in Unity Editor callback that draws wireframe debugging bounds natively within the Scene View.
        /// This ensures effortless visual tracking of the invisible ground sensing trigger.
        /// </summary>
        private void OnDrawGizmosSelected()
        {
            if (groundCheckPoint != null)
            {
                // Changes the color of the upcoming gizmo rendering
                Gizmos.color = Color.green;

                // Draw a matching outline of our exact mathematical ground bounding shape
                Gizmos.DrawWireCube(groundCheckPoint.position, groundCheckSize);
            }
        }
    }
}