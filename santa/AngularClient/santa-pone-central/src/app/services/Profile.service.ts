import { Injectable } from '@angular/core';
import { Profile, ProfileRecipient } from 'src/classes/profile';
import { BehaviorSubject } from 'rxjs';
import { MessageHistory } from 'src/classes/message';
import { SantaApiGetService } from './santaApiService.service';
import { MapService } from './mapService.service';
import { Message } from '@angular/compiler/src/i18n/i18n_ast';

@Injectable({
  providedIn: 'root'
})
export class ProfileService {

  constructor(private SantaApiGet: SantaApiGetService, private ApiMapper: MapService) { }

  private _gettingProfile: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  get gettingProfile()
  {
    return this._gettingProfile.asObservable();
  }

  private _gettingHistories: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  get gettingHistories()
  {
    return this._gettingHistories.asObservable();
  }

  private _gettingSelectedHistory: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  get gettingSelectedHistory()
  {
    return this._gettingSelectedHistory.asObservable();
  }

  private _gettingGeneralHistory: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  get gettingGeneralHistory()
  {
    return this._gettingGeneralHistory.asObservable();
  }

  private _profile: BehaviorSubject<Profile>= new BehaviorSubject(new Profile());
  private _chatHistories: BehaviorSubject<Array<MessageHistory>> = new BehaviorSubject([])
  private _selectedHistory: BehaviorSubject<MessageHistory>= new BehaviorSubject(new MessageHistory());
  private _generalHistory: BehaviorSubject<MessageHistory>= new BehaviorSubject(new MessageHistory());


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

  // * METHODS * //
  // passed option softUpdate boolean for determining if something is a hard or soft update. Used for telling app is spinners should be used or not
  public async getProfile(email: string, isSoftUpdate: boolean = false)
  {
    if(!isSoftUpdate)
    {
      this._gettingProfile.next(true);
    }

    var data = await this.SantaApiGet.getProfile(email).toPromise().catch(err => {console.log(err); this._gettingProfile.next(false);});
    let profile = this.ApiMapper.mapProfile(data);
    this.updateProfile(profile);

    this._gettingProfile.next(false);
  }
  public async getHistories(clientID: string, isSoftUpdate: boolean = false)
  {
    if(!isSoftUpdate)
    {
      this._gettingHistories.next(true);
    }

    let histories: Array<MessageHistory> = []
    var data = await this.SantaApiGet.getAllMessageHistoriesByClientID(clientID).toPromise().catch((err) => {console.log(err);});
    for(let i = 0; i < data.length; i++)
    {
      histories.push(this.ApiMapper.mapMessageHistory(data[i]))
    }

    this.updateChatHistories(histories);
    this._gettingHistories.next(false);

  }
  public async gatherGeneralHistory(conversationClientID: string, subjectID: string, isSoftUpdate: boolean = false)
  {
    if(!isSoftUpdate)
    {
      this._gettingGeneralHistory.next(true);
    }

    var data = await this.SantaApiGet.getClientMessageHistoryBySubjectIDAndXrefID(conversationClientID ,subjectID, null).toPromise().catch((err) => {console.log(err);});

    this.updateGeneralHistory(this.ApiMapper.mapMessageHistory(data));
    this._gettingGeneralHistory.next(false);
  }
  public async getSelectedHistory(conversationClientID: string, subjectID: string, relationXrefID: string, isSoftUpdate: boolean = false)
  {
    if(!isSoftUpdate)
    {
      this._gettingSelectedHistory.next(true);
    }

    // If the relationship is not null, update the selected history, else, update the general one as well, since that is the one that is being requested
    if(relationXrefID != null && relationXrefID != undefined)
    {
      var data = await this.SantaApiGet.getClientMessageHistoryBySubjectIDAndXrefID(conversationClientID, subjectID, relationXrefID).toPromise().catch((err) => {console.log(err);});

      this.updateSelectedHistory(this.ApiMapper.mapMessageHistory(data));
    }
    else
    {
      var data = await this.SantaApiGet.getClientMessageHistoryBySubjectIDAndXrefID(conversationClientID, subjectID, null).toPromise().catch((err) => {console.log(err);});

      this.updateGeneralHistory(this.ApiMapper.mapMessageHistory(data));
      this.updateSelectedHistory(this.ApiMapper.mapMessageHistory(data));
    }


    this._gettingSelectedHistory.next(false);
  }

}
