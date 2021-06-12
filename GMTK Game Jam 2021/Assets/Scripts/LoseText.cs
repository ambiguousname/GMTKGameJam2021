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
        string[] reviewerNames = { "Movie Reviews.com", "Film Maker's Toolkit", "The IMDB Trivia Page", "Random Individual We Found Off the Street", "Lavender Gooms", "Ghee Buttersnaps", "Sh'Dynasty" }; ;
        string[] descriptions;
        if (isPositive) {
            ratings = new string[] { "8/10", "100/100", "9/10", ":)", "★★★★★" };
            descriptions = new string[] { "I think I liked it.", "I had no idea giving real guns with live rounds to actors could be so enjoyable.", "I am completely satisfied with this outcome.", "It was good."};
        } else {
            ratings = new string[] { "1/10", "100/5", "0/10", ":(", "☆☆☆☆☆" };
            descriptions = new string[] { "The best thing I can say is that it was short.", "The main character died, and we were all left waiting in the audience for about an hour or so.",
                "When it ended, I saw my own negative review being shown on the screen. I think I'm trapped in some sort of nightmarish-", "So bad it's bad.", "Makes heavy use of ugly CGI.",
            "The camera shake was intense. Perhaps too intense.", "I don't want to talk about it."};
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
        
    }
}
