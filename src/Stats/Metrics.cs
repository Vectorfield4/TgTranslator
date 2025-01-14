using Prometheus;
using TgTranslator.Interfaces;

namespace TgTranslator.Stats;

public class Metrics : IMetrics
{
    private readonly Counter _groupCharacters;
    private readonly Counter _groupMessages;
    private readonly Counter _totalMessages;
    private readonly Counter _translatorApiCalls;
    private readonly Counter _translatorApiCharacters;

    public Metrics()
    {
        _totalMessages = Prometheus.Metrics.CreateCounter("total_messages", "Total messages");

        _groupMessages = Prometheus.Metrics.CreateCounter("group_messages", "Group messages", "group_id");
        _groupCharacters = Prometheus.Metrics.CreateCounter("group_characters", "Group Characters", "group_id");

        _translatorApiCalls = Prometheus.Metrics.CreateCounter("translator_api_calls", "Translator API calls", "group_id");
        _translatorApiCharacters = Prometheus.Metrics.CreateCounter("translator_api_characters", "Translator API characters", "group_id");
    }
        
    #region IMetrics Members

    public void HandleMessage() => TotalMessagesInc();

    public void HandleGroupMessage(long groupId, int charactersCount)
    {
        GroupMessagesInc(groupId);
        GroupCharactersInc(groupId, charactersCount);
    }

    public void HandleTranslatorApiCall(long groupId, int charactersCount)
    {
        TranslatorApiCallsInc(groupId);
        TranslatorApiCharactersInc(groupId, charactersCount);
    }

    #endregion

    private void TotalMessagesInc() => _totalMessages.Inc();
    private void GroupMessagesInc(long groupId) => _groupMessages.WithLabels(groupId.ToString()).Inc();
    private void GroupCharactersInc(long groupId, int charactersCount) => _groupCharacters.WithLabels(groupId.ToString()).Inc(charactersCount);
    private void TranslatorApiCallsInc(long groupId) => _translatorApiCalls.WithLabels(groupId.ToString()).Inc();
    private void TranslatorApiCharactersInc(long groupId, int charactersCount) => _translatorApiCharacters.WithLabels(groupId.ToString()).Inc(charactersCount);
}