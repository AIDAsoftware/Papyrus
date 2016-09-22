'use strict'
 
const gulp = require('gulp');
const browserify = require('browserify');
const vinylSource = require('vinyl-source-stream');
const babelify = require('babelify');
const eslint = require('gulp-eslint');
const glob = require('globule');

const sourcesFolder = './src/main.js';
const outputBundleFile = 'bundle.js';
const distFolder = './dist';

const testsFolder = './test/specs/**/*.js';
const outputTestFile = 'specs.js';
const distTestFolder = './test/dist';

gulp.task('default', ['lint', 'build']);
 
gulp.task('lint', () => {
    return gulp.src(sourcesFolder)
        .pipe(eslint())
        .pipe(eslint.format())
});
 
gulp.task('build', function() {
    browserify(sourcesFolder)
        .transform(babelify, {presets: ["es2015", "react"], plugins: ["transform-decorators-legacy"]})
        .bundle()
        .pipe(vinylSource(outputBundleFile))
        .pipe(gulp.dest(distFolder));
});

gulp.task('test', function() {
    browserify(glob.find(testsFolder))
        .transform(babelify, {presets: ["es2015", "react"]})
        .bundle()
        .pipe(vinylSource(outputTestFile))
        .pipe(gulp.dest(distTestFolder));
});