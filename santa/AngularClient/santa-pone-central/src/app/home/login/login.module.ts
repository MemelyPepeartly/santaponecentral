import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ReactiveFormsModule } from '@angular/forms';

import { AppComponent } from '../../app.component';
import { LoginComponent } from './login.component';

import { CustomMaterialModule } from '../../core/material.module';

@NgModule({
  imports:      [ BrowserModule, ReactiveFormsModule, CustomMaterialModule, BrowserAnimationsModule],
  declarations: [ AppComponent, LoginComponent ],
  bootstrap:    [ AppComponent ]
})
export class AppModule { 
}
