using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraScript : MonoBehaviour {

	void Start () {

		float TARGET_WIDTH = 850.0f;
		float TARGET_HEIGHT = 500.0f;
		int PIXELS_TO_UNITS = 50; // 1:1 Radio de pixeles a unidades
		float desiredRatio = TARGET_WIDTH / TARGET_HEIGHT;
		float currentRatio = (float)Screen.width/(float)Screen.height;

		if(currentRatio >= desiredRatio)

		{
			//La resolucion tiene bastante ancho, solo necesitamos usar el alto para determinar el tamaño de la camara
			Camera.main.orthographicSize = TARGET_HEIGHT / 2 / PIXELS_TO_UNITS;
		}
		else
		{
			//Nuestra camara necesita hacer menos zoom para que el alto de las imagenes entren en la pantalla
			//Se determina que tan grande necesita ser, y luego eso se aplica
			float differenceInSize = desiredRatio / currentRatio;
			Camera.main.orthographicSize = TARGET_HEIGHT / 2 / PIXELS_TO_UNITS * differenceInSize;

		}

	}
}
