using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.XPath;

namespace ConsoleApplication1
{
    class Assignment1
    {

                 //queries
            public XmlNodeList Query1(XmlDocument xmlDoc)// returns all the movies
            {
                return xmlDoc.SelectNodes("//movie");
            }

            public XmlNodeList Query2(XmlDocument xmlDoc)// returns all the  movies after 2014
            {
                return xmlDoc.SelectNodes("//movie[year>2014]");
            }

            public XmlNodeList Query3(XmlDocument xmlDoc, String actorFirstName, String actorLastName)// returns all the awards of all TV-shows of an actor
            {
                return xmlDoc.SelectNodes("//TV-show/actors/actor[first-name='" + actorFirstName + "' and last-name='" + actorLastName + "']/awards/award");
            }
            public XmlNodeList Query4(XmlDocument xmlDoc)// returns all the TV-shows with more than one seasons
            {
                return xmlDoc.SelectNodes("//TV-show[seasons[count(season)>1]]");
            }
            public int Query5(XmlDocument xmlDoc, String genre)// retuens the amount of movies in the genre
            {
                XmlNodeList movByGenre = xmlDoc.SelectNodes("//movie[genre='" + genre + "']");
                return movByGenre.Count;
            }
            public int Query6(XmlDocument xmlDoc, String yearOfBirth, int amountOfAwards)// returns the amount of different actors that were born after the year and that have more than the award amount in one movie or one TV-show
            {
                XmlNodeList result = xmlDoc.SelectNodes("//actors/actor[count(awards/*) > " + amountOfAwards + " and year-of-birth > '" + yearOfBirth + "']/first-name" +
                    "| //actors/actor[count(awards/*) > " + amountOfAwards + " and year-of-birth > '" + yearOfBirth + "']/last-name");
                ArrayList AL = new ArrayList();
                String name = "";
                 for(int i=0; i<result.Count;i+=2)
                {
                    name = result[i].InnerText +" "+ result[i + 1].InnerText;
                    if(!AL.Contains(name))
                        AL.Add(name);
                }

                return AL.Count;
            }
            public XmlNodeList Query7(XmlDocument xmlDoc, int amountOfEpisodes)// returns the TV-shows that have more than the amount of epidods in all its seasons
            {
                return xmlDoc.SelectNodes("//TV-show[sum(seasons/season/episodes/text()) > " + amountOfEpisodes + "]");
            }

            //insertions
            public void InsertTVShow(XmlDocument xmlDoc, String name, String genre, String year)
            {
                XmlElement newTvShow = CreateNewXmlElement(xmlDoc, "TV-show", "");
                XmlElement n = CreateNewXmlElement(xmlDoc, "name", name);
                XmlElement g = CreateNewXmlElement(xmlDoc, "genre", genre);
                XmlElement y = CreateNewXmlElement(xmlDoc, "year", year);

                newTvShow.AppendChild(n);
                newTvShow.AppendChild(g);
                newTvShow.AppendChild(y);


                XmlNode TvS = xmlDoc.SelectSingleNode("/Netflix/TV-shows");
                if (TvS == null)
                {
                    XmlElement TvShows = CreateNewXmlElement(xmlDoc, "TV-shows", "");
                    XmlNode x = xmlDoc.SelectSingleNode("/Netflix");
                    x.AppendChild(TvShows);
                    TvS = xmlDoc.SelectSingleNode("/Netflix/TV-shows");
                }

                TvS.AppendChild(newTvShow);
            }

            public void InsertMovie(XmlDocument xmlDoc, String name, String genre, String year)
            {
                XmlElement newMovie = CreateNewXmlElement(xmlDoc, "movie", "");
                XmlElement n = CreateNewXmlElement(xmlDoc, "name", name);
                XmlElement g = CreateNewXmlElement(xmlDoc, "genre", genre);
                XmlElement y = CreateNewXmlElement(xmlDoc, "year", year);

                newMovie.AppendChild(n);
                newMovie.AppendChild(g);
                newMovie.AppendChild(y);

                XmlNode movies = xmlDoc.SelectSingleNode("//movies");
                if (movies == null)
                {
                    XmlElement mvs = CreateNewXmlElement(xmlDoc, "movies", "");
                    XmlNode x = xmlDoc.SelectSingleNode("/Netflix");
                    x.AppendChild(mvs);
                    movies = xmlDoc.SelectSingleNode("/Netflix/movies");
                }
                movies.AppendChild(newMovie);
            }
            public void InsertActorToMovie(XmlDocument xmlDoc, String movieName, String actorFirstName, String actorLastName,
            String actorBirthYear)
            {
                XmlElement newActor = CreateNewXmlElement(xmlDoc, "actor", "");
                XmlElement n = CreateNewXmlElement(xmlDoc, "first-name", actorFirstName);
                XmlElement g = CreateNewXmlElement(xmlDoc, "last-name", actorLastName);
                XmlElement y = CreateNewXmlElement(xmlDoc, "year-of-birth", actorBirthYear);

                newActor.AppendChild(n);
                newActor.AppendChild(g);
                newActor.AppendChild(y);

                XmlNode actCheck = xmlDoc.SelectSingleNode("//movie[name='" + movieName + "']/actors");
                if (actCheck == null)
                {
                    XmlElement actTitle = CreateNewXmlElement(xmlDoc, "actors", "");
                    XmlNode x = xmlDoc.SelectSingleNode("//movie[name='" + movieName + "']");
                    x.AppendChild(actTitle);
                    actCheck = xmlDoc.SelectSingleNode("//movie[name='" + movieName + "']/actors");
                }
                actCheck.AppendChild(newActor);
            }
            public void InsertActorToTVShow(XmlDocument xmlDoc, String showName, String actorFirstName, String actorLastName,
            String actorBirthYear)
            {
                if (xmlDoc == null)
                    return;
                XmlNode allTv = xmlDoc.SelectSingleNode("Netflix/TV-shows");
                if (allTv == null)
                    return;
                XmlNode actor = xmlDoc.SelectSingleNode("Netflix/TV-shows/TV-show[name ='" + showName + "']/actors");
                if (actor == null)
                {
                    XmlNode actors = xmlDoc.SelectSingleNode("Netflix/TV-shows/TV-show[name ='" + showName + "']");
                    XmlElement xmlNodeAc = xmlDoc.CreateElement("actors");
                    actors.AppendChild(xmlNodeAc);
                }
                actor = xmlDoc.SelectSingleNode("Netflix/TV-shows/TV-show[name ='" + showName + "']/actors");

                XmlElement xmlNodethisAc = xmlDoc.CreateElement("actor");
                XmlElement xmlAtt1 = CreateNewXmlElement(xmlDoc, "first-name", actorFirstName);
                xmlNodethisAc.AppendChild(xmlAtt1);
                XmlElement xmlAtt2 = CreateNewXmlElement(xmlDoc, "last-name", actorLastName);
                xmlNodethisAc.AppendChild(xmlAtt2);
                XmlElement xmlAtt3 = CreateNewXmlElement(xmlDoc, "year-of-birth", actorBirthYear);
                xmlNodethisAc.AppendChild(xmlAtt3);
                actor.AppendChild(xmlNodethisAc);
            }


            public void InsertSeasonToTVShow(XmlDocument xmlDoc, String showName, String numberOfEpisodes)
            {
                XmlElement newSeason = CreateNewXmlElement(xmlDoc, "season", "");
                XmlElement n = CreateNewXmlElement(xmlDoc, "episodes", numberOfEpisodes);

                newSeason.AppendChild(n);


                XmlNode sesCheck = xmlDoc.SelectSingleNode("//TV-show[name='" + showName + "']/seasons");
                if (sesCheck == null)
                {
                    XmlElement actTitle = CreateNewXmlElement(xmlDoc, "seasons", "");
                    XmlNode x = xmlDoc.SelectSingleNode("//TV-show[name='" + showName + "']");
                    x.AppendChild(actTitle);
                    sesCheck = xmlDoc.SelectSingleNode("//TV-show[name='" + showName + "']/seasons");
                }
                sesCheck.AppendChild(newSeason);
            }
            public void InsertAwardToActorInMovie(XmlDocument xmlDoc, String actorFirstName, String actorLastName, String movieName, String awardCategory, String yearOfWinning)
            {
                XmlElement newAward = CreateNewXmlElement(xmlDoc, "award", "");
                XmlElement l = CreateNewXmlElement(xmlDoc, "category", awardCategory);
                XmlElement r = CreateNewXmlElement(xmlDoc, "year", yearOfWinning);

                newAward.AppendChild(l);
                newAward.AppendChild(r);

                XmlNode awdCheck = xmlDoc.SelectSingleNode("//movie[name='" + movieName +
                "']/actors/actor[first-name ='" + actorFirstName + "' and last-name='" + actorLastName + "']/awards");
                if (awdCheck == null)
                {
                    XmlElement actTitle = CreateNewXmlElement(xmlDoc, "awards", "");
                    XmlNode x = xmlDoc.SelectSingleNode("//movie[name='" + movieName +
                    "']/actors/actor[first-name ='" + actorFirstName + "' and last-name='" + actorLastName + "']");
                    x.AppendChild(actTitle);
                    awdCheck = xmlDoc.SelectSingleNode("//movie[name='" + movieName +
                    "']/actors/actor[first-name ='" + actorFirstName + "' and last-name='" + actorLastName + "']/awards");
                }
                awdCheck.AppendChild(newAward);
            }
            public void InsertAwardToActorInTVShow(XmlDocument xmlDoc, String actorFirstName, String actorLastName, String showName, String awardCategory, String yearOfWinning)
            {
                XmlElement newAward = CreateNewXmlElement(xmlDoc, "award", "");
                XmlElement n = CreateNewXmlElement(xmlDoc, "category", awardCategory);
                XmlElement g = CreateNewXmlElement(xmlDoc, "year", yearOfWinning);

                newAward.AppendChild(n);
                newAward.AppendChild(g);

                XmlNode awdCheck = xmlDoc.SelectSingleNode("//TV-show[name='" + showName +
                "']/actors/actor[first-name ='" + actorFirstName + "' and last-name='" + actorLastName + "']/awards");
                if (awdCheck == null)
                {
                    XmlElement actTitle = CreateNewXmlElement(xmlDoc, "awards", "");
                    XmlNode x = xmlDoc.SelectSingleNode("//TV-show[name='" + showName +
                    "']/actors/actor[first-name ='" + actorFirstName + "' and last-name='" + actorLastName + "']");
                    x.AppendChild(actTitle);
                    awdCheck = xmlDoc.SelectSingleNode("//TV-show[name='" + showName +
                    "']/actors/actor[first-name ='" + actorFirstName + "' and last-name='" + actorLastName + "']/awards");
                }
                awdCheck.AppendChild(newAward);
            }

        public static XmlElement CreateNewXmlElement(XmlDocument xmlDoc, string elemName, string elemValue)
        {
            XmlElement newXmlElem = xmlDoc.CreateElement(elemName);
            newXmlElem.InnerText = elemValue;
            return newXmlElem;
        }

        private void exampleOfCreateXML()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<DataBaseImplementationCourse/>");//insert your XML file path here
            XmlElement newXmlElem = xmlDoc.CreateElement("Lecturer");
            newXmlElem.InnerText = "Dr. Robert Moskovitch";
            xmlDoc.FirstChild.AppendChild(newXmlElem);
            newXmlElem = xmlDoc.CreateElement("TeachingAssistants");
            xmlDoc.FirstChild.AppendChild(newXmlElem);
            XmlNode tempXmlNode = newXmlElem;
            newXmlElem = CreateNewXmlElement(xmlDoc, "TeachingAssistant", "TeachingAssistant");
            newXmlElem.InnerText = "Guy Shitrit";
            tempXmlNode.AppendChild(newXmlElem);
            newXmlElem = CreateNewXmlElement(xmlDoc, "TeachingAssistant", "Ofir Dvir");
            tempXmlNode.AppendChild(newXmlElem);

            XmlNode xmlNode = xmlDoc.SelectSingleNode("DataBaseImplementationCourse/TeachingAssistants");
            XmlNodeList xmlNodesList = xmlDoc.SelectNodes("DataBaseImplementationCourse/TeachingAssistants/TeachingAssistant");
        }

        }
    }
