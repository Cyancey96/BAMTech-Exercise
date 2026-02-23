import { bootstrapApplication } from '@angular/platform-browser';
import { AppComponent } from './app/app.component';
import { provideRouter, Routes } from '@angular/router';
import { PersonListComponent } from './app/components/person-list/person-list.component';
import { AstronautDutyComponent } from './app/components/atronaut-duty/astronaut-duty.component';

const routes: Routes = [
  { path: 'people', component: PersonListComponent },
  { path: 'duties', component: AstronautDutyComponent },
  { path: '', redirectTo: 'people', pathMatch: 'full' },
  { path: '**', redirectTo: 'people', pathMatch: 'full' }
];

bootstrapApplication(AppComponent, {
  providers: [
    provideRouter(routes)
  ]
}).catch(err => console.error(err));