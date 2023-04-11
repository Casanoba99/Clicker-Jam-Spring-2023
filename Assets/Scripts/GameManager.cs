using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager gm;
    private void Awake()
    {
        gm = this;
    }
    #endregion

    Coroutine aumentoCafe, restaCafe;

    int cafeAbsoluto;

    public float tiempo;
    public int cafeTotal;

    [Header("Textos Cafe")]
    public TextMeshProUGUI cafeNumero;
    public string cafesNumeroTxt;
    public TextMeshProUGUI cafesSegundo;
    public string cafesSegundoTxt;
    public TextMeshProUGUI total;
    public string totalTxt;

    [Header("Mejoras Activas")]
    public int cafeXclick;

    [Header("Mejoras Pasivas")]
    public int cafeXsegundo;

    [Header("Resta de cafe")]
    public int tiempoSinClick;
    public bool restar;

    [Header("Particulas")]
    public GameObject tazaPfb;
    public Transform taza;

    [Header("Multiplicador")]
    public int multiploUltimo;
    public ComprarMejora[] mejoras;

    [Header("Desplegable")]
    public bool desplegar;
    public Animator desplegable;
    public TextMeshProUGUI desplegarTMP;

    [Header("Audio")]
    public AudioMixerGroup mixer;
    public AudioSource tazaAudio;

    void Start()
    {
        cafeXclick = 1;
        multiploUltimo = 1;

        cafeNumero.text = cafeTotal + cafesNumeroTxt;
        cafesSegundo.text = cafesSegundoTxt + cafeXsegundo;
        cafeAbsoluto = 0;
        total.text = totalTxt + cafeAbsoluto;
    }

    void Update()
    {
        // Capturas
        if (Input.GetKeyDown(KeyCode.F12))
        {
            string date = System.DateTime.Now.ToString("dd-MM-yy_HH-mm-ss");
            ScreenCapture.CaptureScreenshot(Application.persistentDataPath + "/Screenshot_" + date + ".png");
        }

        if (cafeXsegundo > 0 && restar == false)
        {
            Start_CafePorSegundo();
        }

        if (tiempo >= tiempoSinClick)
        {
            restar = true;
            Start_RestarCafe();
        }
        else
        {
            tiempo += Time.deltaTime;
        }
    }

    #region COSAS DE HACER CLIC
    // Mejora activa
    public void ClickCafe()
    {
        // Sonido
        tazaAudio.Play();
        
        // Reinicio del tiempo y parar Coro
        tiempo = 0;
        restar = false;
        StopCoroutine(RestarCafe());
        restaCafe = null;

        // Particulas taza
        _ = Instantiate(tazaPfb, taza.position, Quaternion.identity, taza);
        cafeTotal += cafeXclick;
        cafeNumero.text = cafeTotal + cafesNumeroTxt;

        cafeAbsoluto += cafeXclick;
        total.text = totalTxt + cafeAbsoluto;
    }

    // Mejora pasiva
    void Start_CafePorSegundo()
    {
        aumentoCafe ??= StartCoroutine(CafePorSegundo());
    }

    IEnumerator CafePorSegundo()
    {
        yield return new WaitForSecondsRealtime(1);

        cafeTotal += cafeXsegundo;
        cafeNumero.text = cafeTotal + cafesNumeroTxt;

        cafeAbsoluto += cafeXsegundo;
        total.text = totalTxt + cafeAbsoluto;

        aumentoCafe = null;
    }

    // Resta cafe
    void Start_RestarCafe()
    {
        restaCafe ??= StartCoroutine(RestarCafe());
    }

    IEnumerator RestarCafe()
    {
        yield return new WaitForSecondsRealtime(1);
        if (cafeTotal > 0) cafeTotal -= cafeXsegundo;
        if (cafeTotal <= 0) cafeTotal = 0;

        cafeNumero.text = cafeTotal + cafesNumeroTxt;

        restaCafe = null;
    }
    #endregion
    #region AUDIO
    // Audio
    public void Musica(bool set)
    {
        if (set == false) mixer.audioMixer.SetFloat("Musica", 0);
        else mixer.audioMixer.SetFloat("Musica", -80);
    }

    public void Sonido(bool set)
    {
        if (set == false) mixer.audioMixer.SetFloat("Sonido", 0);
        else mixer.audioMixer.SetFloat("Sonido", -80);
    }
    #endregion

    public void Multiplicador(int m)
    {
        for (int i = 0; i < mejoras.Length; i++)
        {
            int precio = mejoras[i].precio / multiploUltimo;
            mejoras[i].precio = precio * m;
            mejoras[i].precioTxt.text = mejoras[i].precio.ToString();
        }

        multiploUltimo = m;
    }

    public void Desplegar()
    {
        desplegar = !desplegar;
        desplegable.SetBool("Mover", desplegar);

        if (desplegar) desplegarTMP.text = "<";
        else desplegarTMP.text= ">";
    }
}
