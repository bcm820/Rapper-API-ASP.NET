function pushToURI(route) {
  return event => {
    event.preventDefault();
    if (route.includes('{')) {
      const paramStart = route.indexOf('{');
      const param = route.substring(paramStart + 1, route.indexOf('}'));
      const input = prompt(
        `Enter parameter: ${route.substring(paramStart)}`,
        ''
      );
      if (!input) return;
      route = route.substring(0, paramStart).concat(input);
    }
    window.location = route;
  };
}

if (performance.navigation.type == 2) location.reload(true);

const elements = document.getElementsByClassName('route');
for (const element of elements) {
  element.addEventListener('click', pushToURI(element.getAttribute('href')));
}
