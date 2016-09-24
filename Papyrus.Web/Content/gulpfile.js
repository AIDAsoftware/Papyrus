'use strict';

const gulp = require('gulp');
const browserify = require('browserify');
const vinylSource = require('vinyl-source-stream');
const babelify = require('babelify');
const eslint = require('gulp-eslint');
const glob = require('globule');
const gulpWatch = require('gulp-watch');
const runSequence = require('run-sequence');
const plumber = require('gulp-plumber');

const sources = {
    files : './src/app/main.js',
    outputFile : 'bundle.js',
    distFolder : './dist'
};

const tests = {
    files : './test/specs/**/*.js',
    outputFile : 'specs.js',
    distFolder : './test/dist'
};

gulp.task('default', ['lint', 'build']);

gulp.task('lint', () => {
    return gulp.src(['**/*.js', '!node_modules/**/*', '!dist/**/*', '!test/dist/**/*'])
        .pipe(eslint())
        .pipe(eslint.format());
});

gulp.task('build', ['lint'], () => {
    build(sources.files, sources.outputFile, sources.distFolder);
});

gulp.task('test', function() {
    build(tests.files, tests.outputFile, tests.distFolder);
});

gulp.task('watch', () => {
    gulpWatch('./src/**/*.js', () => {
        runSequence('build');
    });
});

function build(files, outputFile, distFolder) {
    browserify(glob.find(files))
        .transform(babelify, {presets: ['es2015', 'react', 'stage-2']})
        .bundle()
        .pipe(plumber())
        .pipe(vinylSource(outputFile))
        .pipe(gulp.dest(distFolder));
}