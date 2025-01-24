import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { AppComponent } from './app.component';
import { CalendarComponent } from './calendar/calendar.component';

const routes: Routes = [
  { path: 'login', component: LoginComponent }, // Domyślna trasa przekierowująca na stronę logowania
  { path: 'calendar', component: CalendarComponent}, // Kalendarz z ochroną
  {path: '', component: LoginComponent, pathMatch: 'full'} // Jeśli trasa nie istnieje, przekierowanie na stronę logowania
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
