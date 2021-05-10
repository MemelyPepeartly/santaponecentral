import { Component, OnInit } from '@angular/core';
import { AuthService } from '../auth/auth.service';
import { BoardEntry, EntryType } from 'src/classes/missionBoards';
import { MissionBoardGathererService } from '../services/gathering services/mission-board-gatherer.service';
import { MissionBoardService } from '../services/api services/mission-board.service';

@Component({
  selector: 'app-mission-boards',
  templateUrl: './mission-boards.component.html',
  styleUrls: ['./mission-boards.component.css']
})
export class MissionBoardsComponent implements OnInit {

  // auth service for authentication
  // missionBoardAPI service for API calls
  // missionBoardService for centralized data calls
  // missionMapper for mapping to types
  constructor(public auth: AuthService,
    public MissionBoardService: MissionBoardService,
    public MissionBoardGatheringService: MissionBoardGathererService) { }

  public profile: any;
  public isAdmin: boolean;
  public isHelper: boolean;

  public allBoardEntries: Array<BoardEntry> = [];
  public allEntryTypes: Array<EntryType> = [];
  get allPostNumbers(): Array<number>
  {
    let array: Array<number> = []
    this.allBoardEntries.forEach((entry: BoardEntry) => {
      array.push(entry.postNumber);
    });
    return array;
  }

  public gettingAllBoardEntries: boolean = false;
  public gettingAllEntryTypes: boolean = false;

  async ngOnInit() {
    this.auth.userProfile$.subscribe(data => {
      this.profile = data;
    });
    this.auth.isAdmin.subscribe((admin: boolean) => {
      this.isAdmin = admin;
    });
    this.auth.isHelper.subscribe((helper: boolean) => {
      this.isHelper = helper;
    });

    this.MissionBoardGatheringService.gettingAllBoardEntries.subscribe((status: boolean) => {
      this.gettingAllBoardEntries = status;
    });
    this.MissionBoardGatheringService.gettingAllEntryTypes.subscribe((status: boolean) => {
      this.gettingAllEntryTypes = status;
    });
    this.MissionBoardGatheringService.allBoardEntries.subscribe((boardEntryArray: Array<BoardEntry>) => {
      this.allBoardEntries = boardEntryArray;
    });
    this.MissionBoardGatheringService.allEntryTypes.subscribe((entryTypeArray: Array<EntryType>) => {
      this.allEntryTypes = entryTypeArray;
    });

    await this.MissionBoardGatheringService.gatherAllBoardEntries();
    await this.MissionBoardGatheringService.gatherAllEntryTypes();
  }
  public async hardRefreshEntryList(successfullyPosted: boolean)
  {
    if(successfullyPosted)
    {
      await this.MissionBoardGatheringService.gatherAllBoardEntries();
    }
  }
  public async softRefreshEntryList(successfullyPosted: boolean)
  {
    if(successfullyPosted)
    {
      await this.MissionBoardGatheringService.gatherAllBoardEntries(true);
    }
  }
  public adminTypes() : Array<EntryType>
  {
    return this.allEntryTypes.filter((entryType: EntryType) => {return entryType.adminOnly});
  }
  public agentTypes() : Array<EntryType>
  {
    return this.allEntryTypes.filter((entryType: EntryType) => {return entryType.adminOnly == false});
  }
  public getAdminEntriesWithType(type: EntryType) : Array<BoardEntry>
  {
    return this.allBoardEntries.filter((boardEntry: BoardEntry) => {return boardEntry.entryType.adminOnly && boardEntry.entryType.entryTypeID == type.entryTypeID})
  }
  public getAgentEntriesWithType(type: EntryType) : Array<BoardEntry>
  {
    return this.allBoardEntries.filter((boardEntry: BoardEntry) => {return !boardEntry.entryType.adminOnly && boardEntry.entryType.entryTypeID == type.entryTypeID})
  }
}
