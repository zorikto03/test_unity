mergeInto(LibraryManager.library, {

  Hello: function () {
    window.alert("Hello, world!");
    console.log("Helo, world");
  },

  GetPlayerData: function(){
    if (player){
      var name = player.getName();
      var photoUrl = player.getPhoto("medium");
      console.log(name);
      console.log(photoUrl);
      myGameInstance.SendMessage('Yandex', 'SetName', name);
      myGameInstance.SendMessage('Yandex', 'SetPhoto', photoUrl);
    } 
    else 
    {
      console.log("player not login");
    }
  },

  CheckRateGame: function(){
    console.log("CheckRateGame function called");

    ysdk.feedback.canReview()
      .then(({ value, reason }) => {
        var canReview = JSON.stringify(value);
        myGameInstance.SendMessage('Yandex', 'ActivateRateGameButton', canReview);
        console.log('feedback.canReview is ' + canReview);
      })
  },

  RateGame: function(){
    ysdk.feedback.canReview()
    .then(({ value, reason }) => {
      if (value) {
        ysdk.feedback.requestReview()
        .then(({ feedbackSent }) => {
          var feedbackSentString = JSON.stringify(feedbackSent);
          myGameInstance.SendMessage('Yandex', 'SetFeedbackValue', feedbackSentString);
          console.log('feedbackSent is ' + feedbackSentString);
        })
      } else {
        console.log(reason);
        var canReview = JSON.stringify(value);
        myGameInstance.SendMessage('Yandex', 'ActivateRateGameButton', canReview);
      }
    })
  },

  SaveExtern: function(data){
    var strObj = UTF8ToString(data);
    var pairs = JSON.parse(strObj);
    console.log(pairs);
    player.setData(pairs);
  },

  LoadExtern: function(){
    if (player){
      player.getData().then(data => {
        const myJSON = JSON.stringify(data);
        myGameInstance.SendMessage('Progres', 'SetPlayerInfo', myJSON);
      });
    }
    else{
      console.log("player not login");
    }      
  },

  SetLeaderBoardScore: function(value){
    ysdk.getLeaderboards()
    .then(lb => {
        // Без extraData
      lb.setLeaderboardScore('MaxScore', value);
    });
  },

  GetLeaderboard: function (){
    ysdk.getLeaderboards()
    .then(lb => {
        // Получение 10 топов и 3 записей возле пользователя
      lb.getLeaderboardEntries('MaxScore', { quantityTop: 10, includeUser: true, quantityAround: 3 })
      .then(res => {
        console.log(res);
        myGameInstance.SendMessage('Yandex', 'ParseLeaderBoard', res);
      });
    });
  },

  ShowFullScreenAdv: function() {
    ysdk.adv.showFullscreenAdv({
      callbacks: {
        onOpen: () => {
          myGameInstance.SendMessage('Yandex', 'GamePauseYandex');
          console.log("------Adv was opened -------");
        },
        onClose: function(wasShown) {
          myGameInstance.SendMessage('Yandex', 'GamePlayYandex');
          console.log("------Adv was closed -------");
        },
        onError: function(error) {
          myGameInstance.SendMessage('Yandex', 'GamePlayYandex');
          console.log(error);
        }
      }
    })
  },

  ShowRewardedVideo: function() {
    ysdk.adv.showRewardedVideo({
      callbacks: {
        onOpen: () => {
          myGameInstance.SendMessage('Yandex', 'GamePauseYandex');
          console.log('Video ad open.');
        },
        onRewarded: () => {
          myGameInstance.SendMessage('Yandex', 'OnRewarded');
          console.log('Rewarded!');
        },
        onClose: () => {
          myGameInstance.SendMessage('Yandex', 'GamePlayYandex');
          console.log('Video ad closed.');
        }, 
        onError: (e) => {
          myGameInstance.SendMessage('Yandex', 'GamePlayYandex');
          console.log('Error while open video ad:', e);
        }
      }
    })
  },

  Login: function(){
    console.log('Login function called');

    var promise = new Promise((resolve, reject) => {
      if (player){
        if (player.getMode() === 'lite'){
          ysdk.auth.openAuthDialog().then(() => {
            console.log('player has been logged in');
            resolve('success');
          })
          .catch(err => {
            console.log('player has not been logged in');
            reject(new Error(err));
          });
        }
        else{
          console.log('player is already logged in');
          resolve('success');
        }
      }
      else{
        console.log('player is null');
        reject('player is null');
      }     
    });

    promise
      .then(value => {
        console.log(value);
        
        myGameInstance.SendMessage('Yandex', 'DisableLoginPanel');

        player.getData().then(data => {
          const myJSON = JSON.stringify(data);
          myGameInstance.SendMessage('Progres', 'SetPlayerInfo', myJSON);
        });
      })
      .catch(value => {
        console.log(value);
      })
  },

  CheckLogin: function() {
    console.log('CheckLogin function called');

    if (player){
      if (player.getMode() === 'lite'){
        console.log('player is not logged in');
      }
      else{
        console.log('player is already logged in');

        myGameInstance.SendMessage('Yandex', 'DisableLoginPanel');

        player.getData().then(data => {
          const myJSON = JSON.stringify(data);
          myGameInstance.SendMessage('Progres', 'SetPlayerInfo', myJSON);
        });
      }
    }
    else{
      console.log('player is null');
    } 
  },

  Logging: function(data){
    var text = UTF8ToString(data);
    console.log(text);
  },

  GetLanguage: function(){
    console.log('GetLanguage function called');
    
    var lang = ysdk.environment.i18n.lang;
    var buffer = _malloc(lengthBytesUTF8(lang) + 1);
    writeStringToMemory(lang, buffer);
    return buffer;
  },
});