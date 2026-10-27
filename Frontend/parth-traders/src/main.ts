import { enableProdMode } from '@angular/core';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { AppModule } from './app/app.module';
import { environment } from './environments/environment';

if (environment.production) {
  enableProdMode();
}

platformBrowserDynamic().bootstrapModule(AppModule)
  .catch(() => {
    const message = document.querySelector('app-root p');
    if (message) message.textContent = 'The store could not start. Reload this page or contact Parth Traders.';
  });
