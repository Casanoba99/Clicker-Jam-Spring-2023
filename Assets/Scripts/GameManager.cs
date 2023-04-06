using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;
    private void Awake()
    {
        gm = this;
    }

    Coroutine aumentoCafe, restaCafe;

    public float tiempo;

    public float cafeTotal;

    [Header("Textos Cafe")]
    public TextMeshProUGUI cafeNumero;
    public string cafesNumeroTxt;
    public TextMeshProUGUI cafesSegundo;
    public string cafesSegundoTxt;

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

    [Header("Audio")]
    public AudioMixerGroup mixer;
    public AudioSource tazaAudio;

    void Start()
    {
        cafeXclick = 1;

        cafeNumero.text = cafeTotal + cafesNumeroTxt;
        cafesSegundo.text = cafesSegundoTxt + cafeXsegundo;
    }

    void Update()
    {
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
        if (cafeTotal > 0) cafeTotal--;
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
}
