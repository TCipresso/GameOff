using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IntroManager : MonoBehaviour
{
    public AudioSource bootSound;
    public GameObject startBox;
    public GameObject powerButton;
    public Image logoImage; // Reference to the logo Image component
    public Image powerButtonImage; // Reference to the power button Image component
    public TextMeshProUGUI textBox1;
    public TextMeshProUGUI textBox2;
    public TextMeshProUGUI textBox3;
    public Animation uiAnimation;
    public float flickerDuration = 0.1f;
    public float typingSpeed1 = 0.02f;
    public float typingSpeed2 = 0.02f;
    public float typingSpeed3 = 0.02f;
    public float fadeDuration = 1.5f; // Duration of each fade in/out
    private bool hasStarted = false;
    public int maxLinesInTerminal = 30;

    private List<string> startupLines1 = new List<string>
    {
        $@"CPU TYPE			: HORSES 300 CPU at 7777 MHz
CACHE Memory                  : 23456847k
ALIAS 		          : 'Unknown'"
    };

    private List<string> startupLines2 = new List<string>
    {
        $@"RAM INSTALLED		:  --YES-- 1023523k
BIOS Version 		:  --V.557.3--
KERNEL Connection		:  --GOOD--
Locating SWMG 		:  --FOUND--
Permissions		:  --744--
Copyright (C) 1972 - 1985, Fistbump Technologies."
    };

    private List<string> startupLines3 = new List<string>
    {
        "LOADING BOOT --0%--",
        "LOADING BOOT --23%--",
        "LOADING BOOT --44%--",
        "LOADING BOOT --78%--",
        "LOADING BOOT --COMPLETE--",
        "",
        "------ RUNNING SYSTEM CHECK ------",
        "$404-FORTRESS --FOUND--",
        "$ALIAS --????--",
        "$USER --????--",
        "$ALLOCATED MEMORY --356594 KB--",
        "$PRIMARY DRIVE --MOUNTED--",
        "$VIRTUAL DRIVE --MOUNT ERROR--",
        "$DRIVER STATUS --OUTDATED--",
        "$CPU USAGE --1%--",
        "$CORE TEMPERATURE --NOMINAL--",
        "",
        "------ INITIALIZING NETWORK PROTOCOL ------",
        "$PINGING LOCALHOST... SUCCESS",
        "$CONNECTING TO SECURE NODE... FAILURE",
        "$RETRYING CONNECTION... FAILURE",
        "$CONNECTING TO BACKUP NODE... SUCCESS",
        "$IP ADDRESS ASSIGNED --192.168.0.2--",
        "$NETWORK STATUS --LIMITED--",
        "",
        "------ SYSTEM DIAGNOSTICS ------",
        "$MEMORY SCAN --BEGIN--",
        "$MEMORY SECTOR 0x000001... OK",
        "$MEMORY SECTOR 0x000002... OK",
        "$MEMORY SECTOR 0x000003... ERROR --CORRUPTED SECTOR--",
        "$MEMORY SCAN COMPLETE",
        "$CORRUPTION DETECTED IN 2 SECTORS",
        "",
        "------ USER DATA INITIALIZATION ------",
        "$USER 1 --AUTHORIZED--",
        "$USER 2 --AUTHORIZED--",
        "$USER 3 --ERROR: UNKNOWN USER",
        "$USER 4 --AUTHORIZED--",
        "$USER STATUS: --PARTIAL ACCESS--",
        "",
        "------ BOOT LOADER COMPLETE ------",
        "$ALLOCATING RESOURCES...",
        "$CPU CORES INITIALIZED",
        "$GPU ACCELERATION --ENABLED--",
        "$ALGORITHM HASHING... COMPLETE",
        "$SYSTEM READY FOR INPUT",
        "",
        "------ SYSTEM WARNINGS ------",
        "$WARNING: FIRMWARE OUTDATED",
        "$WARNING: INSUFFICIENT DISK SPACE",
        "$WARNING: CRITICAL SECURITY UPDATES AVAILABLE",
        "$WARNING: UNKNOWN USB DEVICE DETECTED",
        "",
        "------ INITIATING SECURE MODE ------",
        "$ENCRYPTING LOCAL FILES...",
        "$ENCRYPTION LEVEL: AES-256",
        "$AUTHORIZATION REQUIRED FOR FINAL SYSTEM ACCESS",
        "$ENTER USER AUTH CODE:",
        "",
        "AUTH CODE DECLINED",
        "------ MUST LOGIN ------",
        "$SYSTEM BOOT SUCCESSFUL",
        "$ENGAGING LOGIN SEQUENCE"
    };

    private void Start()
    {
        textBox1.text = "";
        textBox2.text = "";
        textBox3.text = "";

        // Initialize the images' opacity
        SetImageAlpha(logoImage, 0);
        SetImageAlpha(powerButtonImage, 0);

        // Start the logo fade-in and fade-out automatically
        StartCoroutine(LogoIntroSequence());
    }

    private void SetImageAlpha(Image image, float alpha)
    {
        Color color = image.color;
        color.a = alpha;
        image.color = color;
    }

    private IEnumerator LogoIntroSequence()
    {
        // Fade in the logo, wait, then fade it out
        yield return StartCoroutine(FadeIn(logoImage));
        yield return new WaitForSeconds(2.0f);
        yield return StartCoroutine(FadeOut(logoImage));

        // Enable and fade in the power button after the logo sequence
        powerButton.SetActive(true);
        yield return StartCoroutine(FadeIn(powerButtonImage));
    }

    public void StartSequence()
    {
        if (hasStarted) return;
        hasStarted = true;

        // Disable the power button to prevent further clicks
        powerButton.SetActive(false);

        // Start the main intro sequence
        StartCoroutine(PlayIntroSequence());
    }

    private IEnumerator PlayIntroSequence()
    {
        // Start the main intro sequence after clicking the power button
        bootSound.Play();
        yield return StartCoroutine(FlickerStartBox());
        yield return StartCoroutine(TypeText(textBox1, startupLines1, typingSpeed1));
        yield return StartCoroutine(TypeText(textBox2, startupLines2, typingSpeed2));
        yield return StartCoroutine(TypeTextWithScroll(textBox3, startupLines3, typingSpeed3));
        uiAnimation.Play("Screen_Slam");
    }

    private IEnumerator FadeIn(Image image)
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(0, 1, elapsedTime / fadeDuration);
            SetImageAlpha(image, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        SetImageAlpha(image, 1);
    }

    private IEnumerator FadeOut(Image image)
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(1, 0, elapsedTime / fadeDuration);
            SetImageAlpha(image, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        SetImageAlpha(image, 0);
    }

    private IEnumerator FlickerStartBox()
    {
        int flickerCount = Random.Range(3, 6);
        for (int i = 0; i < flickerCount; i++)
        {
            startBox.SetActive(!startBox.activeSelf);
            yield return new WaitForSeconds(flickerDuration);
        }
        startBox.SetActive(true);
    }

    private IEnumerator TypeText(TextMeshProUGUI textBox, List<string> lines, float typingSpeed)
    {
        foreach (var line in lines)
        {
            foreach (char c in line)
            {
                textBox.text += c;
                yield return new WaitForSeconds(typingSpeed);
            }
            textBox.text += "\n";
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator TypeTextWithScroll(TextMeshProUGUI textBox, List<string> lines, float typingSpeed)
    {
        List<string> displayedLines = new List<string>();

        foreach (var line in lines)
        {
            string currentLine = "";
            foreach (char c in line)
            {
                currentLine += c;
                textBox.text = string.Join("\n", displayedLines) + "\n" + currentLine;
                yield return new WaitForSeconds(typingSpeed);
            }

            displayedLines.Add(line);

            if (displayedLines.Count > maxLinesInTerminal)
            {
                displayedLines.RemoveAt(0);
            }
            textBox.text = string.Join("\n", displayedLines);

            yield return new WaitForSeconds(0.005f);
        }
    }
}
