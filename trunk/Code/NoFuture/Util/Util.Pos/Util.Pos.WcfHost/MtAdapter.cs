using System.IO;
using System.Linq;
using edu.stanford.nlp.tagger.maxent;
using NoFuture.Exceptions;
using NoFuture.Tools;

namespace NoFuture.Util.Pos.WcfHost
{
    /// <summary>
    /// see [http://www-nlp.stanford.edu/nlp/javadoc/javanlp/edu/stanford/nlp/tagger/maxent/MaxentTagger.html]
    /// </summary>
    public static class MtAdapter
    {
        private static MaxentTagger _mt;

        public static MaxentTagger Mt
        {
            get
            {
                if (_mt == null)
                    InitMaxentTagger();
                return _mt;
            }
        }

        public static string ToTaggedString(this string commonEnText)
        {
            if (string.IsNullOrWhiteSpace(commonEnText))
                return null;

            return Mt.tagString(commonEnText);
        }

        public static TagsetBase[] ToPosTagsets(this string commonEnText)
        {
            var taggedText = ToTaggedString(commonEnText);
            if (string.IsNullOrWhiteSpace(taggedText))
                return null;

            TagsetBase[] tagsOut;
            if (!PtTagset.TryParse(taggedText, out tagsOut))
                return null;
            return tagsOut;
        }

        internal static void InitMaxentTagger()
        {
            var model = PathToModel();
            if(string.IsNullOrWhiteSpace(model))
                throw new ItsDeadJim("Could not resolve the .tagger models path.");
            _mt = new MaxentTagger(model);

        }

        internal static string PathToModel()
        {
            if (string.IsNullOrWhiteSpace(JavaTools.StanfordPostTaggerModels) || !Directory.Exists(JavaTools.StanfordPostTaggerModels))
            {
                throw new RahRowRagee("The global variable NoFuture.JavaTools.StanfordPostTaggerModels " +
                                      "needs to be assigned to the directory containing the tagger's models.");
            }

            var di = new DirectoryInfo(JavaTools.StanfordPostTaggerModels);
            var models = di.GetFiles("*.tagger");
            if (models == null || models.Length <= 0)
                throw new RahRowRagee(
                    string.Format(
                        "The directory '{0}' does not contain any files " +
                        "ending in the expected extension of .tagger",
                        JavaTools.StanfordPostTaggerModels));

            var model = models.OrderByDescending(x => x.Length).First();
            return model.FullName;
        }
    }
}
