namespace ADOPSE.Services;

public interface IOpenAiService{

Task<string> CompleteSentence(string text);

Task<string> test();
}