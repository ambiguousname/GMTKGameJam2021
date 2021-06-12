using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoseText : MonoBehaviour
{
    public bool isPositive = false;
    // Start is called before the first frame update
    void Start()
    {
        string[] ratings;
        string[] reviewerNames = { "Movie Reviews.com", "Film Maker's Toolkit", "The IMDB Trivia Page", "Random Individual We Found Off the Street", "Lavender Gooms", "Ghee Buttersnaps", "My Dad" }; ;
        string[] descriptions;
        if (isPositive) {
            ratings = new string[] { "8/10", "100/100", "9/10", ":)", "★★★★★", "B+", "A-", "10 Thumbs Up" };
            descriptions = new string[] { "I think I liked it.", "I had no idea giving real guns with live rounds to actors could be so enjoyable.", "I am completely satisfied with this outcome.", "It was good.",
            "I feel asleep, but my kids seemed to like it, so it's good enough for me.", "I finally learned what it means to Die Hard with a Vengeance (1995).", "Stanley Kubrick was a director.", 
            "Movies are now obsolete. The future is squares shooting at other squares.", "Better than the Emoji Movie."};
        } else {
            ratings = new string[] { "1/10", "2/10", "100/5", "0/10", ":(", "☆☆☆☆☆", "2 Thumbs Down", "1 Middle Finger", "F-" };
            descriptions = new string[] { "The best thing I can say is that it was short.", "The main character died, and we were all left waiting in the audience for about an hour or so.",
                "When it ended, I saw my own negative review being shown on the screen. I think I'm trapped in some sort of nightmarish-", "So bad it's bad.", "Makes heavy use of ugly CGI.",
            "The camera shake was intense. Perhaps too intense.", "I don't want to talk about it.", "Out of all the movies I've seen by this director, this one was by far the worst.",
                "This movie aspires for mediocrity... and almost achieves it.", "The movie was stupendously, horrifically [...] good.",
                "I think I hit myself over the head with a hammer. What's going on? Did I watch a movie recently?", "I am completely disappointed in whoever made this.", "Ha ha ha ha ha... ha... ha."};
        }
        var randomone = Random.Range(0, ratings.Length);
        var randomtwo = Random.Range(0, reviewerNames.Length);
        var randomthree = Random.Range(0, descriptions.Length);
        GetComponent<Text>().text = ratings[randomone];
        this.transform.parent.GetChild(1).GetComponent<Text>().text = "\"" + descriptions[randomthree] + "\"";
        this.transform.parent.GetChild(2).GetComponent<Text>().text = " - " + reviewerNames[randomtwo];
    }

    // Update is called once per frame
    void Update()
    {
        // Make sure that we're always paused while this is up.
        Time.timeScale = 0;
    }
}
