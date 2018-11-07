using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomGeneratedTest : MonoBehaviour {

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
	public Text errorText;
	public Text successText;

	public Text score_text;
	private int score;

	// Use this for initialization
	void Start () {

		score = 0;
		score_text.text = "Punts : " + score;

		//Per si hem estat fent proves i els haviem activat
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
		while (true) {

			//primer deixem tot en l'estat inicial
			endRound = false;
			errors = 0;

			//Generem pregunta i respostes
			GenerateQAndA();
			question_anchor.gameObject.SetActive(true);

			//Esperem els segons que toqui
			yield return wait(); //Aixi en comptes de waitforseconds per poder pintar el timer

			//Amagar l'enunciat i mostrar opcions
			question_anchor.gameObject.SetActive(false);
			lButton.gameObject.SetActive (true);
			mButton.gameObject.SetActive (true);
			rButton.gameObject.SetActive (true);

			//esperem fins que 2 preguntes incorrectes o 1 correcte
			yield return new WaitUntil(() => endRound == true);
		}
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

	public void CheckAnswer (Button b) { //la funcio la criden els botons

		if (b.GetComponentInChildren<Text> ().text == question_anchor.text) { //resposta correcta sumem score i passem a la pregunta seguent
			score++;
			score_text.text = "Punts : " + score;
			StartCoroutine(EndRoundMessage(successText));
		} else if (errors == 1) { //Segona resposta incorrecta passem a la pregunta seguent
			StartCoroutine(EndRoundMessage(errorText));
		} else { //primera resposta incorrecta eliminem resposta
			errors++;
			b.gameObject.SetActive (false);
		}
	}

	void GenerateQAndA () {

		//Generem opcions
		lButton.GetComponentInChildren<Text> ().text = Random.Range (-1000, 1001).ToString();
		mButton.GetComponentInChildren<Text> ().text = Random.Range (-1000, 1001).ToString();
		rButton.GetComponentInChildren<Text> ().text = Random.Range (-1000, 1001).ToString();

		//triem quina de les opcions és la correcta
		int chooseAnswer = Random.Range (0, 3);
		switch (chooseAnswer) {
		case 0:
			question_anchor.text = lButton.GetComponentInChildren<Text> ().text;
			break;
		case 1:
			question_anchor.text = mButton.GetComponentInChildren<Text> ().text;
			break;
		case 2:
			question_anchor.text = rButton.GetComponentInChildren<Text> ().text;
			break;
		default:
			Debug.Log("something went terribly wrong");
			break;
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
