var jsonfile = require('jsonfile'); 
var util = require('util');


var file = 'src/Mediator.Net/project.json'; 
var file2 = 'src/Mediator.Net.Autofac/project.json';
var file3 = 'src/Mediator.Net.Autofac.Test/project.json';
var file4 = 'src/Mediator.Net.IoCTestUtil/project.json';
var file5 = 'src/Mediator.Net.StructureMap/project.json';
var file6 = 'src/Mediator.Net.StructureMap.Test/project.json';
var file7 = 'src/Mediator.Net.Test/project.json'; 
var file8 = 'src/Mediator.Net.Middlewares.Serilog/project.json'; 
var buildNumber = process.env.APPVEYOR_BUILD_VERSION;
var baseVersion = '1.0.34';

jsonfile.readFile(file, function (err, project) { 
project.version = buildNumber; 
jsonfile.writeFile(file, project, {spaces: 2}, function(err) { 
console.error(err); 
}); 
});

jsonfile.readFile(file2, function (err, project) { 
project.version = buildNumber; 
project.dependencies['Mediator.Net'] = baseVersion;
jsonfile.writeFile(file2, project, {spaces: 2}, function(err) { 
console.error(err); 
}); 
});

jsonfile.readFile(file3, function (err, project) { 
project.version = buildNumber; 
project.dependencies['Mediator.Net'] = baseVersion;
project.dependencies['Mediator.Net.IoCTestUtil'] = baseVersion;
project.dependencies['Mediator.Net.Autofac'] = baseVersion;
jsonfile.writeFile(file3, project, {spaces: 2}, function(err) { 
console.error(err); 
}); 
});

jsonfile.readFile(file4, function (err, project) { 
project.version = buildNumber; 
project.dependencies['Mediator.Net'] = baseVersion;
jsonfile.writeFile(file4, project, {spaces: 2}, function(err) { 
console.error(err); 
}); 
});

jsonfile.readFile(file5, function (err, project) { 
project.version = buildNumber; 
project.dependencies['Mediator.Net'] = baseVersion;
console.log(file5);
jsonfile.writeFile(file5, project, {spaces: 2}, function(err) { 
console.error(err); 
}); 
});

jsonfile.readFile(file6, function (err, project) { 
project.version = buildNumber; 
project.dependencies['Mediator.Net'] = baseVersion;
project.dependencies['Mediator.Net.IoCTestUtil'] = baseVersion;
project.dependencies['Mediator.Net.StructureMap'] = baseVersion;
jsonfile.writeFile(file6, project, {spaces: 2}, function(err) { 
console.error(err); 
}); 
});

jsonfile.readFile(file7, function (err, project) { 
project.version = buildNumber; 
project.dependencies['Mediator.Net'] = baseVersion;
jsonfile.writeFile(file7, project, {spaces: 2}, function(err) { 
console.error(err); 
}); 
});

jsonfile.readFile(file8, function (err, project) { 
project.version = buildNumber; 
project.dependencies['Mediator.Net'] = baseVersion;
jsonfile.writeFile(file8, project, {spaces: 2}, function(err) { 
console.error(err); 
}); 
});