import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { provideAuth0 } from '@auth0/auth0-angular';
import { authHttpInterceptorFn } from '@auth0/auth0-angular';

import { routes } from './app.routes';

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }), 
    provideRouter(routes),
    provideHttpClient(
      withInterceptors([authHttpInterceptorFn])
    ),
    provideAuth0({
      domain: 'dev-gp57sox40kt34si8.us.auth0.com',
      clientId: 'sHrj4kGgCL39jsZu2hiCMdZlBv41yt60',
      authorizationParams: {
        redirect_uri: window.location.origin,
        audience: 'https://motomanager.api'
      },
      httpInterceptor: {
        allowedList: [
          'http://localhost:10582/api/*'
        ]
      }
    })
  ]
};
