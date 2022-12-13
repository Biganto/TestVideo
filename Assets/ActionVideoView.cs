using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class ActionVideoView : MonoBehaviour {
    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] AudioSource audioSource;
    [SerializeField] RawImage image;
    [SerializeField] GameObject element;

    private string uri; //uri устанавливается сразу же как только изменился state и позволяет продолжить загрузку видео если оверлей нажат дважды

    public void Render(string video) {
        if (!string.IsNullOrEmpty(video) && video != uri) {
            uri = video;
            StopAllCoroutines();
            StartCoroutine(PlayVideo(uri));
        }
        else if (string.IsNullOrEmpty(video) && !string.IsNullOrEmpty(uri)) {
            StopAllCoroutines();
            videoPlayer.Pause();
            audioSource.Pause();
            uri = null;
            element.gameObject.SetActive(false);
        }
    }

    private IEnumerator PlayVideo(string uri) {
        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = uri;
        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
        videoPlayer.EnableAudioTrack(0, true);
        videoPlayer.SetTargetAudioSource(0, audioSource);
        videoPlayer.Prepare();

        while (!videoPlayer.isPrepared) {
            yield return null;
        }

        videoPlayer.Play();
        audioSource.Play();

        element.gameObject.SetActive(true);
        image.texture = videoPlayer.texture;
    }
}
