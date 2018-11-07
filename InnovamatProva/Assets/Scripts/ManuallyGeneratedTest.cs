using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManuallyGeneratedTest : MonoBehaviour {

	//Per poder organitzar les respostes en un array
	[System.Serializable]
	public struct buttonAnswers {
		public string lButton_text;
		public string mButton_text;
		public string rButton_text;
	}

	//Les preguntes i respostes
	public string [] Questions;
	public buttonAnswers[] options;
	public string[] answers;

	//Els objectes del UI on estaran les preguntes i respostes
	public Text question_anchor;
	public Button lButton;
	public Button mButton;
	public Button rButton;

	//Per fer el contador i poder mostrar els segons
	public float waitTime;
	private float waitCounter;
	public Image timer_ui;

	//per passar de pregunta
	private bool endRound;
	private int errors;
	public Text score_text;
	private int score;

	private int index;

	public Text errorText;
	public Text successText;

	// Use this for initialization
	void Start () {

		score = 0;
		score_text.text = "Punts : " + score;

		//Per si hem estat fent proves al editor posar-ho tot com toca
		question_anchor.text = Questions [0];
		lButton.gameObject.SetActive (false);
		mButton.gameObject.SetActive (false);
		rButton.gameObject.SetActive (false);

		StartCoroutine (TestRoutine ());

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator TestRoutine () {

		//Recorrem totes les preguntes
		while (index < Questions.Length) {

			endRound = false;
			errors = 0;
			//primer mostrem l'enunciat i amaguem botons
			question_anchor.text = Questions [index];

			//Esperem els segons que toqui
			yield return wait(); //Aixi en comptes de waitforseconds per poder pintar el timer

			//Amagar l'enunciat
			question_anchor.text = "";

			//Mostrar botons
			lButton.gameObject.SetActive (true);
			mButton.gameObject.SetActive (true);
			rButton.gameObject.SetActive (true);
			//Mostrar respostes
			lButton.GetComponentInChildren<Text> ().text = options [index].lButton_text;
			mButton.GetComponentInChildren<Text> ().text = options [index].mButton_text;
			rButton.GetComponentInChildren<Text> ().text = options [index].rButton_text;

			//esperem fins que 2 preguntes incorrectes o 1 correcte
			yield return new WaitUntil(() => endRound == true);

			index++;
		}

		//Perque sino esborrem les coses a la ultima pregunta encara podem clickar les respostes i hi ha problemes de out of range
		lButton.gameObject.SetActive (false);
		mButton.gameObject.SetActive (false);
		rButton.gameObject.SetActive (false);
		question_anchor.text = "";

	}

	IEnumerator wait () {
		waitCounter = waitTime;
		while (waitCounter > 0)
		{
			waitCounter -= Time.deltaTime;
			timer_ui.fillAmount = waitCounter/5;
			yield return null;
		}
	}

	public void CheckAnswer (Button b) {
		
		if (b.GetComponentInChildren<Text> ().text == answers [index]) { //resposta correcta sumem score i passem a la pregunta seguent
			score++;
			score_text.text = "Punts : " + score;
			StartCoroutine (EndRoundMessage (successText));
		} else if (errors == 1) { //Segona resposta incorrecta passem a la pregunta seguent
			StartCoroutine(EndRoundMessage(errorText));
		} else { //primera resposta incorrecta eliminem resposta
			errors++;
			b.gameObject.SetActive (false);
		}
	}

	IEnumerator EndRoundMessage (Text t) {
		lButton.gameObject.SetActive (false);
		mButton.gameObject.SetActive (false);
		rButton.gameObject.SetActive (false);

		t.gameObject.SetActive (true);
		yield return new WaitForSeconds (0.75f);
		t.gameObject.SetActive (false);

		endRound = true;
	}

}
