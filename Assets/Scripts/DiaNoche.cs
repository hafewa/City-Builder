﻿using UnityEngine;
using UnityEngine.UI;

public class DiaNoche : MonoBehaviour {

    public Idiomas idioma;

    public Gradient luzColorDia;

    public float maximaIntensidad = 1f;
    public float minimaIntensidad = 0f;
    public float minimoPunto = -0.2f;

    public Light sol;
    public float segundosDiaVelocidad1 = 720f;
    public float segundosDiaVelocidad2 = 360f;

    [Range(0, 1)]
    public float tiempoDia = 0;
    public float tiempoTotalDias = 1;

    [HideInInspector]
    public float tiempoMultiplicador = 1f;

    [SerializeField]
    private Colocar colocar;

    [SerializeField]
    private Text dias;

    [SerializeField]
    private Text reloj;

    public int velocidad;

    [SerializeField]
    private Ciudad ciudad;

    private int contadorHoras = 0;

    public bool encender;

    public Panel panelPausa;
    public Panel panelPlay1;
    public Panel panelPlay2;

    void Update()
    {
        ciudad.ActualizarUI(false);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (velocidad != 0)
            {
                velocidad = 0;
            }
            else
            {
                velocidad = 1;
            }
        }

        if (velocidad != 0)
        {
            float segundosDia = 0f;

            if (velocidad == 1)
            {
                segundosDia = segundosDiaVelocidad1;
            }
            else if (velocidad == 2)
            {
                segundosDia = segundosDiaVelocidad2;
            }

            tiempoDia += (Time.deltaTime / segundosDia);

            if (tiempoDia > 0.98f)
            {
                tiempoDia = 0;
                tiempoTotalDias += 1;
            }
         
            if (encender == true)
            {    
                if ((tiempoDia > 0.7f && tiempoDia <= 0.99f) || (tiempoDia > 0 && tiempoDia < 0.3f))
                {
                    colocar.ComprobarLuces(encender);
                    encender = false;               
                }
            }
            else
            {
                if (tiempoDia <= 0.7f && tiempoDia >= 0.3f)
                {
                    colocar.ComprobarLuces(encender);
                    encender = true;
                }
            }
        }

        if (idioma.CogerCadena("day") != null)
        {
            dias.text = string.Format(idioma.CogerCadena("day").ToLower() + " {0}", Mathf.Round(tiempoTotalDias));
        }       

        ActualizarReloj();
        ActualizarSol();
    }

    void ActualizarReloj()
    {
        float hora = tiempoDia / 0.041666666f;
        float minutos = tiempoDia / 0.00069444444f;
      
        minutos = Mathf.Round(minutos) - (Mathf.Round(hora) * 60) + 30;

        if (contadorHoras != (int)Mathf.Round(hora))
        {
            ciudad.ActualizarUI(true);
        }

        contadorHoras = (int)Mathf.Round(hora);

        string horaS = string.Format("{0}", Mathf.Round(hora));

        if (horaS == "24")
        {
            horaS = "0";        
        }

        string minutosS = string.Format("{0}",minutos);

        if (minutosS.Length == 1)
        {
            minutosS = "0" + minutosS;
        }

        reloj.text = string.Format("{0}:{1}",horaS,minutosS);
    }

    void ActualizarSol()
    {
        sol.transform.localRotation = Quaternion.Euler((tiempoDia * 360f) - 90, 0, 0);

        float dot = Mathf.Clamp01((Vector3.Dot(sol.transform.forward, Vector3.down) - minimoPunto) / tiempoDia);
        float i = ((maximaIntensidad - minimaIntensidad) * dot) + minimaIntensidad;

        sol.intensity = i;
        sol.color = luzColorDia.Evaluate(tiempoDia);

        RenderSettings.ambientLight = sol.color;
    }

    public void VelocidadMarchas(int nuevaVelocidad)
    {
        velocidad = nuevaVelocidad;

        panelPausa.gameObject.GetComponent<Image>().color = new Color(255f, 255f, 255f, 50f / 255f);
        panelPlay1.gameObject.GetComponent<Image>().color = new Color(255f, 255f, 255f, 50f / 255f);
        panelPlay2.gameObject.GetComponent<Image>().color = new Color(255f, 255f, 255f, 50f / 255f);

        panelPausa.volverColor = false;
        panelPlay1.volverColor = false;
        panelPlay2.volverColor = false;

        if (nuevaVelocidad == 0)
        {
            panelPausa.gameObject.GetComponent<Image>().color = new Color(0.08f, 0.4f, 0.58f);
            panelPausa.volverColor = true;
        }
        else if (nuevaVelocidad == 1)
        {
            panelPlay1.gameObject.GetComponent<Image>().color = new Color(0.08f, 0.4f, 0.58f);
            panelPlay1.volverColor = true;
        }
        else if (nuevaVelocidad == 2)
        {
            panelPlay2.gameObject.GetComponent<Image>().color = new Color(0.08f, 0.4f, 0.58f);
            panelPlay2.volverColor = true;
        }
    }

    public void ActualizarLuces()
    {
        if ((tiempoDia > 0.7f && tiempoDia <= 0.99f) || (tiempoDia > 0 && tiempoDia < 0.25f))
        {
            encender = true;
        }
        else
        {
            encender = false;
        }
    }

    public bool EstadoEncendidoLuces()
    {
        if ((tiempoDia > 0.7f && tiempoDia <= 0.99f) || (tiempoDia > 0 && tiempoDia < 0.25f))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
