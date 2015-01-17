# DOTNET_VERSION = 3.5
ifeq ($(DOTNET_VERSION), 3.5)
	COMPILER = gmcs -langversion:3 -define:DOTNET_35

	JSON = lib/Newtonsoft.Json/Net35/Newtonsoft.Json.dll
	C5 = lib/C5/1.1.1/C5.dll
else
	COMPILER = dmcs

	JSON = lib/Newtonsoft.Json/Net45/Newtonsoft.Json.dll
	C5 = lib/C5/2.2/C5.dll
endif

SRC = $(shell find . -name "*.cs" -type f)

CSC = $(COMPILER) -define:MONO $(SRC) -reference:$(C5),$(JSON) -target:exe
NAME = TopKSeqPattMiner.exe

DEBUG = bin/debug
RELEASE = bin/release

LIB = $(JSON) $(C5)

define COMPILE
	mkdir -p $2
	cp $(LIB) $2
	${CSC} $1 -out:$2/$(NAME)
endef

define CLEAN
	rm $1/$(NAME)
endef

all: debug release

debug: $(DEBUG)/$(NAME)

release: $(RELEASE)/$(NAME)

$(DEBUG)/$(NAME): $(SRC) $(LIB)
	$(call COMPILE,-debug -define:DEBUG -checked,$(DEBUG))

$(RELEASE)/$(NAME): $(SRC) $(LIB)
	$(call COMPILE,-optimize,$(RELEASE))

clean: clean_debug clean_release

clean_debug:
	$(call CLEAN,$(DEBUG))

clean_release:
	$(call CLEAN,$(RELEASE))
