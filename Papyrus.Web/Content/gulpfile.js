'use strict'
 
const gulp = require('gulp');
const browserify = require('browserify');
const vinylSource = require('vinyl-source-stream');
const babelify = require('babelify');
const eslint = require('gulp-eslint');
const glob = require('globule');
const gulpWatch = require('gulp-watch');
const runSequence = require('run-sequence');

const mainJs = './src/app/main.js';
const outputBundleFile = 'bundle.js';
const distFolder = './dist';

const testsFolder = './test/specs/**/*.js';
const outputTestFile = 'specs.js';
const distTestFolder = './test/dist';

gulp.task('default', ['lint', 'build']);
 
gulp.task('lint', () => {
    return gulp.src(mainJs)
        .pipe(eslint())
        .pipe(eslint.format())
});
 
gulp.task('build', ['lint'], build);

gulp.task('test', function() {
    browserify(glob.find(testsFolder))
        .transform(babelify, {presets: ["es2015", "react"]})
        .bundle()
        .pipe(vinylSource(outputTestFile))
        .pipe(gulp.dest(distTestFolder));
});

// Remember to use Plumber to reconstruct the watcher
gulp.task('watch', () => {
    gulpWatch('./src/**/*.js', () => {
        runSequence('build');
    });
});

function build() {
    browserify(mainJs)
        .transform(babelify, {presets: ["es2015", "react"], plugins: ["transform-decorators-legacy"]})
        .bundle()
        .pipe(vinylSource(outputBundleFile))
        .pipe(gulp.dest(distFolder));  
}