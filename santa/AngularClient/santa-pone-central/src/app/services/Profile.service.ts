import { Injectable } from '@angular/core';
import { Profile, ProfileRecipient } from 'src/classes/profile';
import { BehaviorSubject } from 'rxjs';
import { MessageHistory } from 'src/classes/message';
import { SantaApiGetService } from './santaApiService.service';
import { MapService } from './mapService.service';

@Injectable({
  providedIn: 'root'
})
export class ProfileService {

  constructor(private SantaApiGet: SantaApiGetService, private ApiMapper: MapService) { }

  private _gettingProfile: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  private _gettingHistories: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  private _gettingSelectedHistory: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  private _gettingGeneralHistory: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);

  private _profile: BehaviorSubject<Profile>= new BehaviorSubject(new Profile);
  private _chatHistories: BehaviorSubject<Array<MessageHistory>> = new BehaviorSubject([])
  private _selectedHistory: BehaviorSubject<MessageHistory>= new BehaviorSubject(new MessageHistory);
  private _generalHistory: BehaviorSubject<MessageHistory>= new BehaviorSubject(new MessageHistory);


  // Profile
  get profile()
  {
    return this._profile.asObservable();
  }
  private updateProfile(profile: Profile)
  {
    this._profile.next(profile);
  }

  // Histories
  get chatHistories()
  {
    return this._chatHistories.asObservable();
  }
  private updateChatHistories(histories: Array<MessageHistory>)
  {
    this._chatHistories.next(histories);
  }

  // Selected History
  get selectedHistory()
  {
    return this._selectedHistory.asObservable();
  }
  private updateSelectedHistory(messageHistory: MessageHistory)
  {
    this._selectedHistory.next(messageHistory)
  }

  // General History
  get generalHistory()
  {
    return this._generalHistory.asObservable();
  }
  private updateGeneralHistory(history: MessageHistory)
  {
    this._generalHistory.next(history);
  }

  // Getting profile boolean
  get gettingProfile()
  {
    return this._gettingProfile.asObservable();
  }
  private updateGettingProfileBool(value: boolean)
  {
    this._gettingProfile.next(value);
  }

  // Getting histories boolean
  get gettingHistories()
  {
    return this._gettingHistories.asObservable();
  }
  private updateGettingHistoriesBool(value: boolean)
  {
    this._gettingHistories.next(value);
  }

  // Getting selected history boolean
  get gettingSelectedHistory()
  {
    return this._gettingSelectedHistory.asObservable();
  }
  private updateGettingSelectedHistoryBool(value: boolean)
  {
    this._gettingSelectedHistory.next(value);
  }

  // Getting general history boolean
  get gettingGeneralHistory()
  {
    return this._gettingGeneralHistory.asObservable();
  }
  private updateGettingGeneralHistoryBool(value: boolean)
  {
    this._gettingGeneralHistory.next(value);
  }

  // * METHODS * //
  // passed option softUpdate boolean for determining if something is a hard or soft update. Used for telling app is spinners should be used or not
  public async getProfile(email)
  {
    this.updateGettingProfileBool(true);

    let profile = this.ApiMapper.mapProfile(await this.SantaApiGet.getProfile(email).toPromise());
    this.updateProfile(profile);

    this.updateGettingProfileBool(false);
  }
  public async getHistories(clientID, isSoftUpdate?: boolean)
  {
    if(!isSoftUpdate)
    {
      this.updateGettingHistoriesBool(true);
    }

    let histories: Array<MessageHistory> = []
    this.SantaApiGet.getAllMessageHistoriesByClientID(clientID).subscribe(res => {
      for(let i = 0; i < res.length; i++)
      {
        histories.push(this.ApiMapper.mapMessageHistory(res[i]))
      }
      this.updateChatHistories(histories);
      this.gatherGeneralHistory(clientID);
      this.updateGettingHistoriesBool(false);

    }, err => {console.log(err); this.updateGettingHistoriesBool(false);});    
    
  }
  public async gatherGeneralHistory(clientID, isSoftGather? : boolean)
  {
    if(!isSoftGather)
    {
      this.updateGettingGeneralHistoryBool(true);
    }

    this.SantaApiGet.getMessageHistoryByClientIDAndXrefID(clientID, null).subscribe(res => {
      this.updateGeneralHistory(this.ApiMapper.mapMessageHistory(res));
      this.updateGettingGeneralHistoryBool(false);
    }, err => {console.log(err); this.updateGettingGeneralHistoryBool(false); })
  }
  public async getSelectedHistory(clientID, relationXrefID)
  {
    this.updateGettingSelectedHistoryBool(true);

    let messageHistory = new MessageHistory();
    this.updateSelectedHistory(messageHistory);
    this.SantaApiGet.getMessageHistoryByClientIDAndXrefID(clientID, relationXrefID).subscribe(res => {
      messageHistory = this.ApiMapper.mapMessageHistory(res);
      this.updateSelectedHistory(messageHistory);
      this.updateGettingSelectedHistoryBool(false);
    },err => {console.log(err); this.updateGettingSelectedHistoryBool(false);}); 
  }

}
