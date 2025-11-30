import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideServerRendering, withRoutes } from '@angular/ssr';
import { provideRouter } from '@angular/router';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { provideClientHydration, withEventReplay } from '@angular/platform-browser';
import { serverRoutes } from './app.routes.server';
import { routes } from './app.routes';

const serverConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideHttpClient(withInterceptorsFromDi()),
    provideClientHydration(withEventReplay()),
    provideServerRendering(withRoutes(serverRoutes))
  ]
};

export const config = serverConfig;
