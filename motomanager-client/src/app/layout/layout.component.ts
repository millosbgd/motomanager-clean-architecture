import { Component, inject, OnInit } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AuthService } from '@auth0/auth0-angular';
import { AsyncPipe } from '@angular/common';
import { KorisnikService } from '../services/korisnik.service';
import { filter, switchMap } from 'rxjs/operators';

@Component({
  selector: 'app-layout',
  standalone: true,
  imports: [RouterModule, AsyncPipe],
  templateUrl: './layout.component.html',
  styleUrl: './layout.component.css'
})
export class LayoutComponent implements OnInit {
  auth: AuthService = inject(AuthService);
  korisnikService: KorisnikService = inject(KorisnikService);

  ngOnInit() {
    // Proveri da li je korisnik registrovan u bazi nakon login-a
    this.auth.user$
      .pipe(
        filter(user => !!user),
        switchMap(user => this.korisnikService.checkIfRegistered(user!.sub!))
      )
      .subscribe({
        next: (result) => {
          this.korisnikService.setRegistered(result.exists);
        },
        error: (err) => {
          console.error('Error checking user registration:', err);
          this.korisnikService.setRegistered(false);
        }
      });
  }

  login() {
    this.auth.loginWithRedirect();
  }

  logout() {
    this.korisnikService.setRegistered(null);
    this.auth.logout({ 
      logoutParams: { 
        returnTo: window.location.origin 
      } 
    });
  }
}
