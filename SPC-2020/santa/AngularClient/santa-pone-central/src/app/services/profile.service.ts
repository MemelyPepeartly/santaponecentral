import { Injectable } from '@angular/core';
import { Profile, ProfileAssignment } from 'src/classes/profile';
import { BehaviorSubject } from 'rxjs';
import { MessageHistory } from 'src/classes/message';
import { ProfileApiService, SantaApiGetService } from './santa-api.service';
import { MapService } from './mapper.service';

@Injectable({
  providedIn: 'root'
})
export class ProfileService {

  constructor(private ProfileApiService: ProfileApiService, private SantaApiGet: SantaApiGetService, private ApiMapper: MapService) { }

  private _gettingClientID: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  get gettingClientID()
  {
    return this._gettingClientID.asObservable();
  }

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

  private _gettingGeneralHistory: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  get gettingGeneralHistory()
  {
    return this._gettingGeneralHistory.asObservable();
  }

  private _gettingAssignments: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  get gettingAssignments()
  {
    return this._gettingAssignments.asObservable();
  }

  private _gettingUnloadedChatHistories: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  get gettingUnloadedChatHistories()
  {
    return this._gettingUnloadedChatHistories.asObservable();
  }

  private _profile: BehaviorSubject<Profile>= new BehaviorSubject(new Profile());
  private _clientID: BehaviorSubject<string>= new BehaviorSubject(undefined);
  private _chatHistories: BehaviorSubject<Array<MessageHistory>> = new BehaviorSubject([]);
  private _unloadedChatHistories: BehaviorSubject<Array<MessageHistory>> = new BehaviorSubject([]);
  private _generalHistory: BehaviorSubject<MessageHistory>= new BehaviorSubject(new MessageHistory());
  private _profileAssignments: BehaviorSubject<Array<ProfileAssignment>>= new BehaviorSubject([]);


  // Profile
  get profile()
  {
    return this._profile.asObservable();
  }
  private updateProfile(profile: Profile)
  {
    this._profile.next(profile);
  }

  // ClientID
  get clientID()
  {
    return this._clientID.asObservable();
  }
  private updateClientID(clientID: string)
  {
    this._clientID.next(clientID);
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

  // General History
  get generalHistory()
  {
    return this._generalHistory.asObservable();
  }
  private updateGeneralHistory(history: MessageHistory)
  {
    this._generalHistory.next(history);
  }

  // Profile Assignments
  get profileAssignments()
  {
    return this._profileAssignments.asObservable();
  }
  private updateProfileAssignments(assignments: Array<ProfileAssignment>)
  {
    this._profileAssignments.next(assignments);
  }

  // Unloaded Chat Histories
  get unloadedChatHistories()
  {
    return this._unloadedChatHistories.asObservable();
  }
  private updateUnloadedChatHistories(histories: Array<MessageHistory>)
  {
    this._unloadedChatHistories.next(histories);
  }

  // * METHODS * //
  // passed option softUpdate boolean for determining if something is a hard or soft update. Used for telling app is spinners should be used or not
  public async getClientIDFromEmail(email: string, isSoftUpdate: boolean = false)
  {
    if(!isSoftUpdate)
    {
      this._gettingClientID.next(true);
    }

    var data = await this.ProfileApiService.getClientIDForProfile(email).toPromise().catch(err => {console.log(err); this._gettingClientID.next(false);});
    this.updateClientID(data);

    this._gettingClientID.next(false);
  }
  public async getProfileByEmail(email: string, isSoftUpdate: boolean = false)
  {
    if(!isSoftUpdate)
    {
      this._gettingProfile.next(true);
    }

    var data = await this.ProfileApiService.getProfileByEmail(email).toPromise().catch(err => {console.log(err); this._gettingProfile.next(false);});
    let profile = this.ApiMapper.mapProfile(data);
    this.updateProfile(profile);

    this._gettingProfile.next(false);
  }
  public async getProfileByID(clientID: string, isSoftUpdate: boolean = false)
  {
    if(!isSoftUpdate)
    {
      this._gettingProfile.next(true);
    }

    var data = await this.ProfileApiService.getProfileByID(clientID).toPromise().catch(err => {console.log(err); this._gettingProfile.next(false);});
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
    var data = await this.SantaApiGet.getAllMessageHistoriesByClientID(clientID).toPromise().catch((err) => {console.log(err); this._gettingHistories.next(false);});
    for(let i = 0; i < data.length; i++)
    {
      histories.push(this.ApiMapper.mapMessageHistory(data[i]))
    }

    this.updateChatHistories(histories);
    this._gettingHistories.next(false);

  }
  public async getUnloadedHistories(clientID: string, isSoftUpdate: boolean = false)
  {
    if(!isSoftUpdate)
    {
      this._gettingUnloadedChatHistories.next(true);
    }

    let histories: Array<MessageHistory> = []
    var data = await this.ProfileApiService.getUnloadedChatHistories(clientID).toPromise().catch((err) => {console.log(err); this._gettingHistories.next(false);});
    for(let i = 0; i < data.length; i++)
    {
      histories.push(this.ApiMapper.mapMessageHistory(data[i]))
    }

    this.updateUnloadedChatHistories(histories);
    this._gettingUnloadedChatHistories.next(false);
  }
  public async gatherGeneralHistory(conversationClientID: string, subjectID: string, isSoftUpdate: boolean = false)
  {
    if(!isSoftUpdate)
    {
      this._gettingGeneralHistory.next(true);
    }

    var data = await this.SantaApiGet.getClientMessageHistoryBySubjectIDAndXrefID(conversationClientID ,subjectID, null).toPromise().catch((err) => {console.log(err); this._gettingGeneralHistory.next(false);});

    this.updateGeneralHistory(this.ApiMapper.mapMessageHistory(data));
    this._gettingGeneralHistory.next(false);
  }
  public async gatherAssignments(clientID: string, isSoftUpdate: boolean = false)
  {
    if(!isSoftUpdate)
    {
      this._gettingAssignments.next(true);
    }

    let profileAssignments: Array<ProfileAssignment> = []
    var data = await this.ProfileApiService.getProfileAssignments(clientID).toPromise().catch((err) => {console.log(err); this._gettingAssignments.next(false);});
    for(let i = 0; i < data.length; i++)
    {
      profileAssignments.push(this.ApiMapper.mapProfileAssignment(data[i]))
    }

    this.updateGeneralHistory(this.ApiMapper.mapMessageHistory(data));
    this._gettingAssignments.next(false);
  }
}
