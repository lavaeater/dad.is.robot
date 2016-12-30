using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Otter;

namespace robot.dad.game.Scenes
{
    /// <summary>
    /// Dispays some kind of cutscene. 
    /// Might be a small video clip or animation, together, perhaps with a text.
    /// Or text + images + music perhaps?
    /// 
    /// Lets go with text + images + music to start, to make it reaal easy
    /// </summary>
    public class IntroScene : Scene
    {
        public Action SceneDone { get; set; }
        //or, a queue of text AND images?
        public Queue<Dictionary<string, Image>> CutSceneData = new Queue<Dictionary<string, Image>>();

        public Queue<string> CrawlData = new Queue<string>();
        private MessageQueueDisplayer MQD;
        public Music IntroMusic = new Music("Music\\bensound-deepblue.ogg");
        public Music IntroSpeech = new Music("Music\\intro.ogg");

        public override void Begin()
        {
            IntroMusic.Play();
            IntroSpeech.Play();
        }

        public IntroScene(Action sceneDone)
        {
            SceneDone = sceneDone;
            //Hardcoded data for now.
            //Start with crawl only untill we can get some images up in this biatch.

            string fileData = File.ReadAllText("Scenes\\intro.txt");

            var paragraphs = fileData.Split(new string[] { "\r\n\r\n"},StringSplitOptions.RemoveEmptyEntries);

            var lines = paragraphs.Select(p => p.Split(new string[] {"\r\n"}, StringSplitOptions.RemoveEmptyEntries));

            MQD = new MessageQueueDisplayer(CrawlData, this, -1400, 0.35f); 

            /*
             * How do we figure this out? 
             * We pop a text mes
             */
            foreach (var paragraph in lines)
            {
                CrawlData.Enqueue("");
                foreach (var line in paragraph)
                {
                    CrawlData.Enqueue(line);
                }
            }
        }

        public override void Update()
        {
            MQD.Update();
            if (MQD.MessagesListIsEmpty)
            {
                SceneDone?.Invoke();
            }
            if (Input.KeyDown(Key.Space))
            {
                SceneDone?.Invoke();
            }
        }

        public override void End()
        {
            IntroMusic.Stop();
            IntroSpeech.Stop();
        }
    }
}
