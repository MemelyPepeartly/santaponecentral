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

  public gettingProfile: boolean = false;
  public gettingHistories: boolean = false;
  public gettingSelectedHistory: boolean = false;

  private _profile: BehaviorSubject<Profile>= new BehaviorSubject(new Profile);
  private _chatHistories: BehaviorSubject<Array<MessageHistory>> = new BehaviorSubject([])
  private _selectedHistory: BehaviorSubject<MessageHistory>= new BehaviorSubject(new MessageHistory);

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

  // * METHODS * //
  // passed option softUpdate boolean for determining if something is a hard or soft update. Used for telling app is spinners should be used or not
  public async getProfile(email)
  {
    let profile = new Profile();
    this.SantaApiGet.getProfile(email).subscribe(res => {
      profile = this.ApiMapper.mapProfile(res);
      this.updateProfile(profile);
    }, err => {console.log(err)});
    
  }
  public async getHistories(clientID, isSoftUpdate?: boolean)
  {
    if(!isSoftUpdate)
    {
      this.gettingHistories = true;
    }

    let histories: Array<MessageHistory> = []
    this.SantaApiGet.getAllMessageHistoriesByClientID(clientID).subscribe(res => {
      for(let i = 0; i < res.length; i++)
      {
        histories.push(this.ApiMapper.mapMessageHistory(res[i]))
      }
      this.updateChatHistories(histories);
      this.gettingHistories = false;
    }, err => {console.log(err); this.gettingHistories = false;});    
    
  }
  public async getSelectedHistory(clientID, relationXrefID)
  {
    this.gettingSelectedHistory = true;

    let messageHistory = new MessageHistory;
    this.SantaApiGet.getMessageHistoryByClientIDAndXrefID(clientID, relationXrefID).subscribe(res => {
      messageHistory = this.ApiMapper.mapMessageHistory(res);
      this.updateSelectedHistory(messageHistory);
      this.gettingSelectedHistory = false;
    },err => {console.log(err); this.gettingSelectedHistory = false;}); 
  }

}
