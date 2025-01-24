import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../auth.service';
import { jwtDecode, JwtPayload } from 'jwt-decode';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent {
  errorMessage: string | null = null;
  username: string = '';
  password: string = '';

  constructor(private authService: AuthService, private router: Router) {}

  async onSubmit(): Promise<void> {
    try {
      const response = await this.authService.login(this.username, this.password).toPromise();
      if (response && response.token) { // Sprawdzamy, czy response zawiera token
        console.log("here");
        localStorage.setItem('token', response.token);
        const decoded = jwtDecode<ExtendedJwtPayload>(response.token); 
        const role  = decoded['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
        if(role) {
          localStorage.setItem('role', role); 
        }
        this.router.navigate(['/calendar']);
      } else {
        throw new Error('Niepoprawna odpowiedź z serwera.');
      }
    } catch (err: any) {
      this.errorMessage = err.error?.message || 'Błąd logowania';
    }
  }
}


interface ExtendedJwtPayload extends JwtPayload {
  'http://schemas.microsoft.com/ws/2008/06/identity/claims/role'?: string;
}