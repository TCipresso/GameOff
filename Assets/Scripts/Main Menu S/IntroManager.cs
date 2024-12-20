using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{
    public AudioSource bootSound;
    public GameObject startBox;
    public GameObject powerButton;
    public GameObject Off;
    public GameObject StartUp;
    public Image logoImage;
    public Image powerButtonImage;
    public TextMeshProUGUI textBox1;
    public TextMeshProUGUI textBox2;
    public TextMeshProUGUI textBox3;
    public Animation uiAnimation;
    public float flickerDuration = 0.1f;
    public float typingSpeed1 = 0.02f;
    public float typingSpeed2 = 0.02f;
    public float typingSpeed3 = 0.02f;
    public float fadeDuration = 1.5f;
    public float offDisableDelay = 1f;
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
        "$IP ADDRESS ASSIGNED --XXX?>?>?XXX--",
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
        "------ LOADING PERIPHERALS ------",
        "$KEYBOARD INITIALIZED",
        "$MOUSE CONNECTED",
        "$DISPLAY ADAPTER... STATUS: LIMITED",
        "$SOUND DEVICE --DRIVER MISSING--",
        "",
        "------ LOADING SECURITY PROTOCOLS ------",
        "$FIREWALL ACTIVATED",
        "$ANTI-MALWARE SCAN --IN PROGRESS--",
        "$SECURITY KEY VALIDATION --PASSED--",
        "$USER ACCESS TOKEN GENERATED",
        "",
        "------ CPU PERFORMANCE CHECK ------",
        "$CORE 1 LOAD --3%--",
        "$CORE 2 LOAD --2%--",
        "$CORE 3 LOAD --5%--",
        "$CORE 4 LOAD --4%--",
        "$CORE 1 LOAD --16%--",
        "$CORE 2 LOAD --24%--",
        "$CORE 3 LOAD --46%--",
        "$CORE 4 LOAD --67%--",
        "$CORE 1 LOAD --78%--",
        "$CORE 2 LOAD --87%--",
        "$CORE 3 LOAD --91%--",
        "$CORE 4 LOAD --99%--",
        "$TEMPERATURE STATUS: NOMINAL",
        "",
        "------ NETWORK STATUS ------",
        "$CONNECTED DEVICES: 4",
        "$TRAFFIC MONITOR: LOW",
        "$VPN STATUS --DISCONNECTED--",
        "$SECURE CHANNEL INITIALIZATION --IN PROGRESS--",
        "",
        "------ INITIALIZING INTERFACE MODULES ------",
        "$UI MODULES --LOADED--",
        "$GRAPHICS ENGINE --STARTED--",
        "$AUDIO ENGINE --ERROR: CODEC NOT FOUND--",
        "$LOGGING SYSTEM --ACTIVE--",
        "",
        "------ FINAL SYSTEM CHECK ------",
        "$DISK SPACE REMAINING: 1432 MB",
        "$RAM USAGE --82%--",
        "$CACHE CLEANUP --INITIATED--",
        "$REBOOT FLAG --SET--",
        "$USER FLAG --SET--",
        "$CHEAT FLAG --SET--",
        "$ZERO FLAG --SET--",
        "",
        "------ SYSTEM WARNINGS ------",
        "$WARNING: POWER SURGE DETECTED",
        "$WARNING: HIGH MEMORY USAGE",
        "$WARNING: NETWORK INTRUSION ATTEMPT BLOCKED",
        "",
        "------ SYSTEM BACKUP INITIATED ------",
        "$DATA MIRRORING --ENABLED--",
        "$BACKUP TO EXTERNAL DRIVE --IN PROGRESS--",
        "$ESTIMATED TIME REMAINING --02:45:30--",
        "",
        "------ AUTHENTICATION MODULES ------",
        "$BIOMETRIC SENSOR --DISABLED--",
        "$2-FACTOR AUTH --PENDING--",
        "$PASSWORD EXPIRATION CHECK --PASSED--",
        "$AUTHORIZATION LEVEL: ADMIN",
        "",
        "------ SYSTEM READY ------",
        "$CPU STATUS: ONLINE",
        "$ALL MODULES INITIALIZED",
        "$STANDBY FOR USER COMMANDS",
        "",
        "------ SYSTEM ENTRY LOG ------",
        "$LAST LOGIN: UNKNOWN",
        "$LAST SHUTDOWN: ABRUPT",
        "$LOGIN ATTEMPTS IN LAST 24H: 3",
        "",
        "$SYSTEM BOOT SUCCESSFUL",
        "$WAITING FOR USER INPUT",
    };


    private void Start()
    {
        textBox1.text = "";
        textBox2.text = "";
        textBox3.text = "";
        SetImageAlpha(logoImage, 0);
        SetImageAlpha(powerButtonImage, 0);
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
        yield return StartCoroutine(FadeIn(logoImage));
        yield return new WaitForSecondsRealtime(2.0f);
        yield return StartCoroutine(FadeOut(logoImage));
        powerButton.SetActive(true);
        yield return StartCoroutine(FadeIn(powerButtonImage));
    }

    public void StartSequence()
    {
        if (hasStarted) return;
        hasStarted = true;
        powerButton.SetActive(false);
        StartCoroutine(PlayIntroSequence());
    }

    private IEnumerator PlayIntroSequence()
    {
        bootSound.Play();
        yield return StartCoroutine(FlickerStartBox());
        yield return StartCoroutine(TypeText(textBox1, startupLines1, typingSpeed1));
        yield return StartCoroutine(TypeText(textBox2, startupLines2, typingSpeed2));
        yield return StartCoroutine(TypeTextWithScroll(textBox3, startupLines3, typingSpeed3));

        uiAnimation.Play("Screen_Slam");
        AnimationClip clip = uiAnimation.GetClip("Screen_Slam");

        if (clip != null)
        {
            yield return new WaitForSecondsRealtime(clip.length);
        }

        if (startBox != null)
        {
            startBox.SetActive(false);
        }

        if (bootSound.isPlaying)
        {
            bootSound.Stop();
        }

        if (Off != null)
        {
            Off.SetActive(true);
            yield return new WaitForSecondsRealtime(offDisableDelay);
            Off.SetActive(false);
        }

        if (StartUp != null)
        {
            StartUp.SetActive(false);
        }

        yield return new WaitForSecondsRealtime(2f);
        SceneManager.LoadScene(1);
    }

    private IEnumerator FadeIn(Image image)
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(0, 1, elapsedTime / fadeDuration);
            SetImageAlpha(image, alpha);
            elapsedTime += Time.unscaledDeltaTime;
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
            elapsedTime += Time.unscaledDeltaTime;
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
            yield return new WaitForSecondsRealtime(flickerDuration);
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
                yield return new WaitForSecondsRealtime(typingSpeed);
            }
            textBox.text += "\n";
            yield return new WaitForSecondsRealtime(0.1f);
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
                yield return new WaitForSecondsRealtime(typingSpeed);
            }

            displayedLines.Add(line);

            if (displayedLines.Count > maxLinesInTerminal)
            {
                displayedLines.RemoveAt(0);
            }
            textBox.text = string.Join("\n", displayedLines);

            yield return new WaitForSecondsRealtime(0.005f);
        }
    }
}
