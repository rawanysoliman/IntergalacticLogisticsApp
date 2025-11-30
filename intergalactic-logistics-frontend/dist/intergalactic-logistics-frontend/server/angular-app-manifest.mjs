
export default {
  bootstrap: () => import('./main.server.mjs').then(m => m.default),
  inlineCriticalCss: true,
  baseHref: '/',
  locale: undefined,
  routes: [
  {
    "renderMode": 2,
    "redirectTo": "/starships",
    "route": "/"
  },
  {
    "renderMode": 2,
    "route": "/starships"
  },
  {
    "renderMode": 2,
    "route": "/book-shipment"
  },
  {
    "renderMode": 2,
    "redirectTo": "/starships",
    "route": "/**"
  }
],
  entryPointToBrowserMapping: undefined,
  assets: {
    'index.csr.html': {size: 5690, hash: '0bb1cdfc45c25b5cab328158683197e86b5eb586f73a63388578c19bc79fb55d', text: () => import('./assets-chunks/index_csr_html.mjs').then(m => m.default)},
    'index.server.html': {size: 1027, hash: '2f9a4624427c08d60f843487fcb27ec6f1df25e3fe121e5720d86182b9627219', text: () => import('./assets-chunks/index_server_html.mjs').then(m => m.default)},
    'starships/index.html': {size: 17436, hash: '177dc74c7c4225672284d09c04bc774ccdd2d49596c217138342f3bc67efabc2', text: () => import('./assets-chunks/starships_index_html.mjs').then(m => m.default)},
    'book-shipment/index.html': {size: 23804, hash: 'bb4f86ece38a6fa68aa601db2f569b53328c1d0d754a5f62d31f4cdad9e57d41', text: () => import('./assets-chunks/book-shipment_index_html.mjs').then(m => m.default)},
    'styles-TOGDDXMX.css': {size: 232309, hash: '7RAMc0XPToM', text: () => import('./assets-chunks/styles-TOGDDXMX_css.mjs').then(m => m.default)}
  },
};
