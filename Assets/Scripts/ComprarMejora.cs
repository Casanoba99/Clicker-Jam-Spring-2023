using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ComprarMejora : MonoBehaviour
{
    bool primeraCompra = true;
    Button boton;
    new AudioSource audio;
    GameObject popUpActual = null;

    public MejorasCafeSO mejora;
    public int precio;
    public int compras;
    public int produccionTotal;
    [Space(5)]
    public TextMeshProUGUI nombre;
    public TextMeshProUGUI precioTxt;
    [Space(5)]
    public Transform canvas;
    public GameObject popUpPfb;
    [Header("Objeto")]
    public Transform objeto;

    private void Start()
    {
        primeraCompra = true;

        boton = GetComponent<Button>();
        audio = GetComponent<AudioSource>();
        canvas = GameObject.Find("Canvas").transform;

        nombre.text = mejora.name;
        precio = mejora.precioInicial;
        precioTxt.text = precio.ToString();
    }

    private void Update()
    {
        if (GameManager.gm.cafeTotal >= precio) boton.interactable = true;
        else boton.interactable = false;
    }

    public void Comprar()
    {
        audio.Play();

        GameManager gm = GameManager.gm;
        // Actualizacion de los textos
        gm.cafeXsegundo += mejora.cafeSegundo * gm.multiploUltimo;
        gm.cafesSegundo.text = gm.cafesSegundoTxt + gm.cafeXsegundo;

        gm.cafeTotal -= precio;
        gm.cafeNumero.text = gm.cafeTotal + gm.cafesNumeroTxt;

        // Incremento del precio
        int incremento;
        incremento = mejora.incrementoPrecio * precio / 100;
        precio += incremento;
        precioTxt.text = precio.ToString();

        compras += 1 * gm.multiploUltimo;
        produccionTotal += mejora.cafeSegundo * gm.multiploUltimo;

        ActualizarPopup();
        if (compras % 10 == 0) ActivarObjetos();

        if (primeraCompra)
        {
            gm.tiempoSinClick += mejora.cafeSegundo;
            primeraCompra = false;
        }
    }

    public void MostrarInfo()
    {
        Vector2 ratonPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        ratonPos.x = .5f;

        popUpActual = Instantiate(popUpPfb, ratonPos, Quaternion.identity, canvas);
        ActualizarPopup();
    }

    public void ActualizarPopup()
    {
        PopUp popup = popUpActual.GetComponent<PopUp>();

        popup.compradosTMP.text = popup.compradosTxt + compras;
        popup.produccionTMP.text = popup.produccionTxt + produccionTotal + popup.produccion1Txt;
    }

    public void OcultarInfo()
    {
        Destroy(popUpActual);
    }

    void ActivarObjetos()
    {
        for (int i = 0; i < objeto.childCount; i++)
        {
            if (objeto.GetChild(i).gameObject.activeSelf == false)
            {
                objeto.GetChild(i).gameObject.SetActive(true);
                return;
            }
        }
    }
}
