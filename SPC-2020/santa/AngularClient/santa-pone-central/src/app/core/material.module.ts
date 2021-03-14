import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatDialogModule } from '@angular/material/dialog';
import { MatDividerModule } from '@angular/material/divider';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatListModule } from '@angular/material/list';
import { MatMenuModule } from '@angular/material/menu';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatRadioModule } from '@angular/material/radio';
import { MatStepperModule } from '@angular/material/stepper';
import { MatTableModule } from '@angular/material/table';
import { MatTabsModule } from '@angular/material/tabs';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatSidenavModule } from '@angular/material/sidenav';
import {MatSelectModule} from '@angular/material/select';
import {MatTooltipModule} from '@angular/material/tooltip';
import {MatChipsModule} from '@angular/material/chips';
import {MatBadgeModule} from '@angular/material/badge';
import {MatProgressBarModule} from '@angular/material/progress-bar';
import {MatAutocompleteModule} from '@angular/material/autocomplete';
import {MatRippleModule} from '@angular/material/core';
import {ScrollingModule} from '@angular/cdk/scrolling';
import {TableVirtualScrollModule} from 'ng-table-virtual-scroll';

@NgModule({
  imports: [
  CommonModule,
  MatToolbarModule,
  MatButtonModule,
  MatCardModule,
  MatInputModule,
  MatDialogModule,
  MatTableModule,
  MatMenuModule,
  MatIconModule,
  MatProgressSpinnerModule,
  MatTabsModule,
  MatDividerModule,
  MatStepperModule,
  MatRadioModule,
  MatPaginatorModule,
  MatListModule,
  MatProgressSpinnerModule,
  DragDropModule,
  MatExpansionModule,
  MatSidenavModule,
  MatSelectModule,
  MatTooltipModule,
  MatChipsModule,
  MatBadgeModule,
  MatProgressBarModule,
  MatAutocompleteModule,
  MatRippleModule,
  ScrollingModule,
  TableVirtualScrollModule
  ],
  exports: [
  CommonModule,
   MatToolbarModule,
   MatButtonModule,
   MatCardModule,
   MatInputModule,
   MatDialogModule,
   MatTableModule,
   MatMenuModule,
   MatIconModule,
   MatProgressSpinnerModule,
   MatTabsModule,
   MatDividerModule,
   MatStepperModule,
   MatRadioModule,
   MatPaginatorModule,
   MatListModule,
   MatProgressSpinnerModule,
   DragDropModule,
   MatExpansionModule,
   MatSidenavModule,
   MatSelectModule,
   MatTooltipModule,
   MatChipsModule,
   MatBadgeModule,
   MatProgressBarModule,
   MatAutocompleteModule,
   MatRippleModule,
   ScrollingModule,
   TableVirtualScrollModule
   ],
})
export class CustomMaterialModule { }
