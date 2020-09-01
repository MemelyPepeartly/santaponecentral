import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { BoardEntry, EntryType } from 'src/classes/missionBoards';
import { MissionBoardAPIService } from './santaApiService.service';
import { MissionMapper } from './mapService.service';

@Injectable({
  providedIn: 'root'
})
export class MissionBoardService {

constructor(private missionBoardAPIService: MissionBoardAPIService, private missionMapper: MissionMapper) { }

  /* STATUS BOOLEAN BEHAVIOR SUBJECTS */
  private _gettingAllBoardEntries: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  get gettingAllBoardEntries()
  {
    return this._gettingAllBoardEntries.asObservable();
  }
  private _gettingAllEntryTypes: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  get gettingAllEntryTypes()
  {
    return this._gettingAllEntryTypes.asObservable();
  }
  /* DATA BEHAVIOR SUBJECTS */
  /*----- AllBoardEntries ----- */
  private _allBoardEntries: BehaviorSubject<Array<BoardEntry>>= new BehaviorSubject([]);
  get allBoardEntries()
  {
    return this._allBoardEntries.asObservable()
  }
  private updateAllBoardEntries(value: Array<BoardEntry>)
  {
    this._allBoardEntries.next(value);
  }
  /*----- AllEntryTypes ----- */
  private _allEntryTypes: BehaviorSubject<Array<EntryType>>= new BehaviorSubject([]);
  get allEntryTypes()
  {
    return this._allEntryTypes.asObservable()
  }
  private updateAllEntryTypes(value: Array<EntryType>)
  {
    this._allEntryTypes.next(value);
  }

  /* GATHERERS */
  public async gatherAllBoardEntries(isSoftUpdate: boolean = false)
  {
    if(!isSoftUpdate)
    {
      this._gettingAllBoardEntries.next(true);
    }
    let boardEntryArray: Array<BoardEntry> = [];

    var data = await this.missionBoardAPIService.getAllBoardEntries().toPromise().catch(err => {console.log(err); this._gettingAllBoardEntries.next(false);});

    for(let i = 0; i < data.length; i++)
    {
      boardEntryArray.push(this.missionMapper.mapBoardEntry(data[i]));
    }
    this._allBoardEntries.next(boardEntryArray);

    this._gettingAllBoardEntries.next(false);
  }
  public async gatherAllEntryTypes(isSoftUpdate: boolean = false)
  {
    if(!isSoftUpdate)
    {
      this._gettingAllEntryTypes.next(true);
    }
    let entryTypeArray: Array<EntryType> = [];

    var data = await this.missionBoardAPIService.getAllEntryTypes().toPromise().catch(err => {console.log(err); this._gettingAllBoardEntries.next(false);});

    for(let i = 0; i < data.length; i++)
    {
      entryTypeArray.push(this.missionMapper.mapEntryType(data[i]));
    }
    this._allEntryTypes.next(entryTypeArray);

    this._gettingAllEntryTypes.next(false);
  }
}
