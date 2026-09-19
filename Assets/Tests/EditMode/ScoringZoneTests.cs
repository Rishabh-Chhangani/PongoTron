using NUnit.Framework;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScoringZoneTests
{
    private GameObject _scoringZoneObject;
    private ScoringZone _scoringZone;

    [SetUp]
    public void SetUp()
    {
        _scoringZoneObject = new GameObject("TestScoringZone");
        _scoringZone = _scoringZoneObject.AddComponent<ScoringZone>();
        _scoringZone.scoreTrigger = new EventTrigger.TriggerEvent();
    }

    [TearDown]
    public void TearDown()
    {
        if (_scoringZoneObject != null)
        {
            Object.DestroyImmediate(_scoringZoneObject);
        }
    }

    [Test]
    public void ScoreTrigger_WhenInvoked_ExecutesAttachedListener()
    {
        // Arrange
        bool listenerInvoked = false;
        BaseEventData receivedEventData = null;

        _scoringZone.scoreTrigger.AddListener((data) =>
        {
            listenerInvoked = true;
            receivedEventData = data;
        });

        BaseEventData testEventData = new BaseEventData(null);

        // Act
        _scoringZone.scoreTrigger.Invoke(testEventData);

        // Assert
        Assert.IsTrue(listenerInvoked, "scoreTrigger should invoke registered listeners.");
        Assert.AreSame(testEventData, receivedEventData, "EventData must be forwarded to the listener.");
    }

    [Test]
    public void ScoreTrigger_MultipleInvocations_FiresEachTime()
    {
        // Arrange
        int invokeCount = 0;
        _scoringZone.scoreTrigger.AddListener((data) =>
        {
            invokeCount++;
        });

        BaseEventData testEventData = new BaseEventData(null);

        // Act
        _scoringZone.scoreTrigger.Invoke(testEventData);
        _scoringZone.scoreTrigger.Invoke(testEventData);
        _scoringZone.scoreTrigger.Invoke(testEventData);

        // Assert
        Assert.AreEqual(3, invokeCount, "scoreTrigger should fire for every call to Invoke.");
    }
}
