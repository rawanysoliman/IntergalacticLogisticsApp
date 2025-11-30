import { bootstrapApplication } from '@angular/platform-browser';
import type { BootstrapContext } from '@angular/platform-browser';
import { App } from './app/app';
import { config } from './app/app.config.server';

const bootstrap = (context?: BootstrapContext) => {
  if (context) {
    return bootstrapApplication(App, config, context);
  }
  return bootstrapApplication(App, config);
};

export default bootstrap;
