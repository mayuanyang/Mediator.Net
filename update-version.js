var jsonfile = require('jsonfile'); 
var util = require('util');


var file = 'src/Mediator.Net/project.json'; 
var file2 = 'src/Mediator.Net.Autofac/project.json';
var file3 = 'src/Mediator.Net.Autofac.Test/project.json';
var file4 = 'src/Mediator.Net.IoCTestUtil/project.json';
var file5 = 'src/Mediator.Net.StructureMap/project.json';
var file6 = 'src/Mediator.Net.StructureMap.Test/project.json';
var file7 = 'src/Mediator.Net.Test/project.json'; 
var buildNumber = '1.0.' + process.env.APPVEYOR_BUILD_VERSION;

jsonfile.readFile(file, function (err, project) { 
project.version = buildNumber; 
jsonfile.writeFile(file, project, {spaces: 2}, function(err) { 
console.error(err); 
}); 
});

jsonfile.readFile(file2, function (err, project) { 
project.version = buildNumber; 
jsonfile.writeFile(file2, project, {spaces: 2}, function(err) { 
console.error(err); 
}); 
});

jsonfile.readFile(file3, function (err, project) { 
project.version = buildNumber; 
jsonfile.writeFile(file3, project, {spaces: 2}, function(err) { 
console.error(err); 
}); 
});

jsonfile.readFile(file4, function (err, project) { 
project.version = buildNumber; 
jsonfile.writeFile(file4, project, {spaces: 2}, function(err) { 
console.error(err); 
}); 
});

jsonfile.readFile(file5, function (err, project) { 
project.version = buildNumber; 
console.log(file5);
jsonfile.writeFile(file5, project, {spaces: 2}, function(err) { 
console.error(err); 
}); 
});

jsonfile.readFile(file6, function (err, project) { 
project.version = buildNumber; 
jsonfile.writeFile(file6, project, {spaces: 2}, function(err) { 
console.error(err); 
}); 
});

jsonfile.readFile(file7, function (err, project) { 
project.version = buildNumber; 
jsonfile.writeFile(file7, project, {spaces: 2}, function(err) { 
console.error(err); 
}); 
});