using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRFootsteps : MonoBehaviour {

	public CapsuleCollider m_Capsule;
	//private Rigidbody m_rb;
	private bool m_Jump, m_PreviouslyGrounded, m_Jumping, m_IsGrounded;
	private Vector2 m_Input;

	//[SerializeField] private AudioClip[] m_FootstepSounds;    // an array of footstep sounds that will be randomly selected from.
	//private AudioSource m_AudioSource;
	//[SerializeField] private AudioClip m_LandSound;           // the sound played when character touches back on ground.
	//[SerializeField] private AudioClip m_JumpSound;           // the sound played when character leaves the ground.
	private AudioManager audm;

	[SerializeField] private bool m_IsWalking;
	[SerializeField] private float m_WalkSpeed;
	[SerializeField] private float m_RunSpeed;
	[SerializeField] [Range(0f, 1f)] private float m_RunstepLenghten;
	[SerializeField] private float m_StepInterval;
	private float m_StepCycle;
	private float m_NextStep;

	// Use this for initialization
	void Start () {
		m_Capsule = GameObject.Find ("[VRTK][AUTOGEN][FootColliderContainer]").GetComponent<CapsuleCollider>();
		//m_rb = GetComponent<Rigidbody>();
		audm = FindObjectOfType<AudioManager>();
	}

	// Update is called once per frame
	void Update()
	{
		if (!m_PreviouslyGrounded && m_IsGrounded)
		{
			//m_AudioSource.clip = m_LandSound;
			//m_AudioSource.Play();
			//audm.Play("m_LandSound");
			m_Jumping = false;
		}
	}

	private void FixedUpdate()
	{
		float speed;
		GetInput(out speed);

		GroundCheck();
		if (m_IsGrounded)
		{
			if (m_Jump)
			{
				//m_AudioSource.clip = m_JumpSound;
				//m_AudioSource.Play();
				audm.Play("m_JumpSound");

				m_Jumping = true;
				m_Jump = false;
			}

		}
		m_Jump = false;
		ProgressStepCycle(speed);
	}


	private void PlayFootStepAudio()
	{
		if (!m_IsGrounded)
		{
			return;
		}
		// pick & play a random footstep sound from the array,
		// excluding sound at index 0
		int n = Random.Range(1, audm.FootStepSounds.Length);
		//m_AudioSource.clip = m_FootstepSounds[n];
		//m_AudioSource.PlayOneShot(m_AudioSource.clip);
		// move picked sound to index 0 so it's not picked next time
		Sound s = audm.FootStepSounds[n];

		audm.PlayFS(n);
		audm.FootStepSounds[n] = audm.FootStepSounds[0];
		audm.FootStepSounds[0] = s;
	}

	private void GroundCheck()
	{
		m_PreviouslyGrounded = m_IsGrounded;
		RaycastHit hitInfo;

		//if (Physics.SphereCast(transform.position, m_Capsule.radius * (1.0f - 0.1f), Vector3.down, out hitInfo,
		//((m_Capsule.height / 2f) - m_Capsule.radius) + 0.1f, Physics.AllLayers, QueryTriggerInteraction.Ignore))

		if (Physics.SphereCast(transform.position + new Vector3(0f, 0.1f, 0f), m_Capsule.radius * (1.0f - 0.1f), Vector3.down, out hitInfo,
		0.1f, Physics.AllLayers, QueryTriggerInteraction.Ignore))

		//if (Physics.SphereCast(transform.position, m_Capsule.radius *1000, Vector3.forward, out hitInfo,
		//					   ((m_Capsule.height) * 5 ) + 0.1f, Physics.AllLayers, QueryTriggerInteraction.Ignore))
		{
			m_IsGrounded = true;
			//m_GroundContactNormal = hitInfo.normal;
		}
		else
		{
			m_IsGrounded = false;
			//m_GroundContactNormal = Vector3.up;
		}
		if (!m_PreviouslyGrounded && m_IsGrounded && m_Jumping)
		{
			m_Jumping = false;
		}
		//m_IsGrounded = true;
	}

	private void ProgressStepCycle(float speed)
	{
		/*if (m_rb.velocity.sqrMagnitude > 0 && (m_Input.x != 0 || m_Input.y != 0))
		{
			m_StepCycle += (m_rb.velocity.magnitude + (speed * (m_IsWalking ? 1f : m_RunstepLenghten))) *
						 Time.fixedDeltaTime;
		}*/
		if ((m_Input.x != 0 || m_Input.y != 0))
		{
			m_StepCycle += ((speed * (m_IsWalking ? 1f : m_RunstepLenghten))) *
				Time.fixedDeltaTime;
		}
		if (!(m_StepCycle > m_NextStep))
		{
			return;
		}

		m_NextStep = m_StepCycle + m_StepInterval;

		PlayFootStepAudio();
	}
	private void GetInput(out float speed)
	{
		// Read input
		float horizontal = Input.GetAxis("Horizontal");
		float vertical = Input.GetAxis("Vertical");

		bool waswalking = m_IsWalking;


		// On standalone builds, walk/run speed is modified by a key press.
		// keep track of whether or not the character is walking or running
		m_IsWalking = !Input.GetKey(KeyCode.LeftShift);

		// set the desired speed to be walking or running
		speed = m_IsWalking ? m_WalkSpeed : m_RunSpeed;
		m_Input = new Vector2(horizontal, vertical);

		// normalize input if it exceeds 1 in combined length:
		if (m_Input.sqrMagnitude > 1)
		{
			m_Input.Normalize();
		}
	}
}
