import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import { PersonListComponent } from './components/person-list/person-list.component';

@NgModule({
  imports: [
    BrowserModule,
    AppComponent,
    PersonListComponent
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }