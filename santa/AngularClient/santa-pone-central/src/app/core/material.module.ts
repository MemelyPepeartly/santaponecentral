import {NgModule} from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  MatButtonModule, MatCardModule, MatDialogModule, MatInputModule, MatTableModule, MatRadioModule, MatPaginatorModule, MatListModule,
MatToolbarModule, MatMenuModule, MatIconModule, MatProgressSpinnerModule, MatTabsModule, MatDividerModule, MatStepperModule
} from '@angular/material';

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
  MatListModule
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
   MatListModule
   ],
})
export class CustomMaterialModule { }
