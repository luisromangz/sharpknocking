dnl Warning: This is an automatically generated file, do not edit!
dnl Process this file with autoconf to produce a configure script.
AC_PREREQ([2.54])
AC_INIT([SharpKnocking], [0.1_i])
AM_INIT_AUTOMAKE([foreign])

dnl pkg-config
AC_PATH_PROG(PKG_CONFIG, pkg-config, no)
if test "x$PKG_CONFIG" = "xno"; then
        AC_MSG_ERROR([You need to install pkg-config])
fi

AC_PROG_INSTALL

AC_PATH_PROG(MCS, mcs, no)
if test "x$MCS" = "xno"; then
        AC_MSG_ERROR([mcs Not found])
fi
AC_PATH_PROG(GMCS, gmcs, no)
if test "x$GMCS" = "xno"; then
        AC_MSG_ERROR([gmcs Not found])
fi


PKG_CHECK_MODULES([GLIB_SHARP_20], [glib-sharp-2.0])
PKG_CHECK_MODULES([GTK_SHARP_20], [gtk-sharp-2.0])
PKG_CHECK_MODULES([GLADE_SHARP_20], [glade-sharp-2.0])


AC_ARG_ENABLE(debug,
	AC_HELP_STRING([--enable-debug],
		[Use 'DEBUG' Configuration [default=YES]]),
		enable_debug=yes, enable_debug=no)
AM_CONDITIONAL(ENABLE_DEBUG, test x$enable_debug = xyes)
if test "x$enable_debug" = "xyes" ; then
	CONFIG_REQUESTED="yes"
fi
AC_ARG_ENABLE(release,
	AC_HELP_STRING([--enable-release],
		[Use 'RELEASE' Configuration [default=NO]]),
		enable_release=yes, enable_release=no)
AM_CONDITIONAL(ENABLE_RELEASE, test x$enable_release = xyes)
if test "x$enable_release" = "xyes" ; then
	CONFIG_REQUESTED="yes"
fi
if test -z "$CONFIG_REQUESTED" ; then
	AM_CONDITIONAL(ENABLE_DEBUG, true)
fi


AC_CONFIG_FILES([
KnockingDaemonControl/Makefile
KnockingDaemon/knockingdaemon
Makefile
Common/Makefile
NetfilterFirewall/Makefile
CommonWidgets/Makefile
Doorman/Makefile
Doorman/doorman
PortKnocker/Makefile
PortKnocker/portknocker
CommonWidgets/commonwidgets.pc
NetfilterFirewall/netfilterfirewall.pc
KnockingDaemon/Makefile
KnockingDaemonControl/knockingdaemoncontrol
Common/common.pc

])

AC_OUTPUT
