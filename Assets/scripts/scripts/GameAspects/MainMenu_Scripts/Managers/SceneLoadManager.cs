using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text;

public class SceneLoadManager : MonoBehaviour
{
    public static SceneLoadManager Instance { get; private set; }

    [SerializeField] private Image transitionImg;
    [SerializeField] private Slider progressBar;
    [SerializeField] private TMP_Text percent;
    [SerializeField] private CanvasGroup canvasGroup;

    private float fadeDuration = 0.5f;

    private float targetProgress;
    private float currentProgress;

    private StringBuilder builder;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            SceneNotifier.OnAspectSelection += SceneLoadAsync; 
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        builder = new StringBuilder();
    }
    private void OnDestroy()
    {
       
        if (Instance == this)
            SceneNotifier.OnAspectSelection -= SceneLoadAsync;
    }

    public void SceneLoadAsync(string Name)
    {
        progressBar.value = 0f;
        percent.text = "0%";
        targetProgress = 0f;
        currentProgress = 0f;
        StartCoroutine(Load(Name));
    }

    private IEnumerator Load(string a)
    {
        yield return FadeScreen(0f, 1f);

        AsyncOperation operation = SceneManager.LoadSceneAsync(a);

        operation.allowSceneActivation = false;

        yield return new WaitForSeconds(1f);

        
     
        while (!operation.isDone)
        {
            targetProgress = Mathf.Clamp01(operation.progress / 0.9f);

            currentProgress = Mathf.MoveTowards(currentProgress, targetProgress,Time.deltaTime * 0.7f);
            
            builder.Clear();

            progressBar.value = currentProgress * 100f;
            builder.Append((currentProgress * 100f ).ToString("0"));
            builder.Append("%");

            percent.text = builder.ToString();

            if (currentProgress >= 1f && operation.progress >= 0.9f)
                operation.allowSceneActivation = true;

            yield return null;
        }

        yield return new WaitForSeconds(0.5f);
      

        yield return FadeScreen(1f, 0f);
    }

    private IEnumerator FadeScreen(float start, float end)
    {
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(start, end, t / fadeDuration);

            yield return null;
        }
        canvasGroup.alpha = end;
    }
}
