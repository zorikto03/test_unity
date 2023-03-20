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
      ysdk.feedback.canReview()
          .then(({ value, reason }) => {
              myGameInstance.SendMessage('Yandex', 'ActivateRateGameButton', value);
          })
    },

    RateGame: function(){
      ysdk.feedback.canReview()
          .then(({ value, reason }) => {
              if (value) {
                  ysdk.feedback.requestReview()
                      .then(({ feedbackSent }) => {
                          myGameInstance.SendMessage('Yandex', 'SetFeedbackValue', feedbackSent);
                          console.log(feedbackSent);
                      })
              } else {
                  console.log(reason);
                  myGameInstance.SendMessage('Yandex', 'ActivateRateGameButton', value);
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
      /**
      var promise = new Promise(function(resolve, reject) {
        initPlayer().then(_player => {
          console.log('auth/initPlayer/then');
            if (_player.getMode() === 'lite') {
                // Игрок не авторизован.
                ysdk.auth.openAuthDialog().then(() => {
                  resolve("result");
                  // Игрок успешно авторизован
                    initPlayer().catch(err => {
                        console.log(err);
                        reject(new Error('error'));
                        // Ошибка при инициализации объекта Player.
                    });
                }).catch(() => {
                    // Игрок не авторизован.
                });
            }
          }).catch(err => {
              console.log(err);
              reject(new Error('error'));
              // Ошибка при инициализации объекта Player.
          });
          resolve("result");
        }
      ).
      then(
        result => {
          myGameInstance.SendMessage('Progres', 'DisableLoginPanel');
          LoadExtern(); 
          GetPlayerData();
      }).
      catch(err => {
        console.log('player not login');
      });
      */
    },

    Logging: function(data){
      var text = UTF8ToString(data);
      console.log(text);
    },
    
  });